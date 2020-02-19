using Framework.Core.Config;
using Framework.Core.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Bus.RabbitMQ
{
    public class BusReceiver : BaseBus, IBusReceiver
    {
        public BusReceiver(IBusConnection busConnection)
            : base(busConnection)
        {
        }

        public Task ReceiveAsync(string queueName, string exchangeName, string topic, Func<MessageInfo, Task> funcAsync)
        {
            var channel = Connection.CreateModel();

            var args = CreateDeadLetterPolicy(channel);

            channel.QueueDeclare(queueName, true, false, false, args);
            channel.BasicQos(0, 10, false);

            if (exchangeName != null)
            {
                var isTopic = !string.IsNullOrEmpty(topic);

                channel.ExchangeDeclare(exchangeName, isTopic ? ExchangeType.Topic : ExchangeType.Fanout, true, false, null);
                channel.QueueBind(queueName, exchangeName, isTopic ? topic : queueName);
            }

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                await HandlingMessage(queueName, funcAsync, ea, channel);
            };

            channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandlingMessage(string queueName, Func<MessageInfo, Task> funcAsync, BasicDeliverEventArgs ea, IModel channel)
        {
            var debugging = Configuration.Debugging.Get();
            var body = ea.Body;
            var messageInfo = new MessageInfo
            {
                ContentName = ea.BasicProperties.ContentType,
                MessageId = ea.BasicProperties.MessageId,
                RequestId = ea.BasicProperties.CorrelationId,
                Priority = ea.BasicProperties.Priority,
                Body = Encoding.UTF8.GetString(body)
            };

            try
            {
                await funcAsync(messageInfo);

                channel.BasicAck(ea.DeliveryTag, false);

                if (debugging)
                    LogHelper.Debug($"Processed {queueName}: {messageInfo.MessageId}");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"Error {queueName}: {ex.Message}.", ex);

                var retryType = HandlingErrorMessage(ea, ex);

                switch (retryType)
                {
                    case ERetryType.Resend:
                        channel.BasicPublish(ea.Exchange, ea.RoutingKey, ea.BasicProperties, ea.Body);
                        channel.BasicAck(ea.DeliveryTag, false);
                        break;

                    case ERetryType.Reject:
                        channel.BasicReject(ea.DeliveryTag, false);
                        break;

                    default:
                        channel.BasicNack(ea.DeliveryTag, false, true);
                        break;
                }

                Task.Delay(500).Wait();
            }
        }

        private ERetryType HandlingErrorMessage(BasicDeliverEventArgs ea, Exception ex)
        {
            var propsMessage = ea.BasicProperties;

            if (propsMessage.Headers == null)
                propsMessage.Headers = new Dictionary<string, object>();

            if (!ea.Redelivered && propsMessage.Headers != null && !propsMessage.Headers.ContainsKey("x-retry-count")) return ERetryType.Retry;

            if (!propsMessage.Headers.ContainsKey("ExceptionMessage"))
            {
                propsMessage.Headers.Add("ExceptionMessage", ex.Message);
                propsMessage.Headers.Add("ExceptionStackTrace", ex.StackTrace);
                propsMessage.Headers.Add("ExceptionInnerException", ex.InnerException?.ToString());
                propsMessage.Headers.Add("Date", DateTime.UtcNow.ToString());
            }
            else
            {
                propsMessage.Headers["ExceptionMessage"] = ex.Message;
                propsMessage.Headers["ExceptionStackTrace"] = ex.StackTrace;
                propsMessage.Headers["ExceptionInnerException"] = ex.InnerException?.ToString();
                propsMessage.Headers["Date"] = DateTime.UtcNow.ToString();
            }

            var requeue = HandlingRetryMessage(propsMessage);

            return requeue;
        }

        private static ERetryType HandlingRetryMessage(IBasicProperties propsMessage)
        {
            if (!propsMessage.Headers.ContainsKey("x-retry-count"))
                propsMessage.Headers.Add("x-retry-count", 0);

            var retryCount = (int)propsMessage.Headers["x-retry-count"];

            if (retryCount <= 3)
            {
                retryCount++;
                propsMessage.Headers["x-retry-count"] = retryCount;
            }
            else
            {
                if (propsMessage.IsPriorityPresent())
                {
                    var priority = propsMessage.Priority;
                    priority += 1;
                    propsMessage.Priority = priority;
                }
            }

            return propsMessage.Priority <= 5 ? ERetryType.Resend : ERetryType.Reject;
        }
    }
}