using Framework.Core.Config;
using Framework.Core.Extensions;
using Framework.Core.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.Bus.RabbitMQ
{
    public class BusPublisher : BaseBus, IBusPublisher
    {
        public BusPublisher(IBusConnection busConnection)
           : base(busConnection)
        {
        }

        public async Task PublishAsync<TMessage>(string exchangeName, IEnumerable<TMessage> messages)
            where TMessage : class, IBusMessage
        {
            await ProcessAsync(exchangeName, string.Empty, messages, true);
        }

        public async Task PublishAsync<TMessage>(string exchangeName, string topic, IEnumerable<TMessage> messages)
            where TMessage : class, IBusMessage
        {
            await ProcessAsync(exchangeName, topic, messages, true);
        }

        public async Task SendAsync<TMessage>(string queueName, IEnumerable<TMessage> messages)
            where TMessage : class, IBusMessage
        {
            await ProcessAsync(queueName, string.Empty, messages, false);
        }

        private Task ProcessAsync<TMessage>(string contextName, string topic, IEnumerable<TMessage> messages, bool isSubscriber)
            where TMessage : class, IBusMessage
        {
            var debugging = Configuration.Debugging.Get();
            var isTopic = !string.IsNullOrEmpty(topic);

            using (var channel = Connection.CreateModel())
            {
                if (isSubscriber)
                    channel.ExchangeDeclare(contextName, isTopic ? ExchangeType.Topic : ExchangeType.Fanout, true, false, null);
                else
                {
                    var args = CreateDeadLetterPolicy(channel);
                    channel.QueueDeclare(contextName, true, false, false, args);
                }

                foreach (var message in messages)
                {
                    var props = channel.CreateBasicProperties();
                    props.MessageId = message.MessageId;
                    props.ContentType = message.ContentName;
                    props.DeliveryMode = 2;

                    var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings()
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                        TypeNameHandling = TypeNameHandling.None,
                    });

                    var messageBytes = json.ToBytes();

                    if (isSubscriber)
                        channel.BasicPublish(contextName, topic, props, messageBytes);
                    else
                    {
                        channel.BasicPublish(string.Empty, contextName, props, messageBytes);
                    }

                    if (debugging)
                        LogHelper.Debug($"Published {contextName}: {message.MessageId}");
                }
            }

            return Task.CompletedTask;
        }
    }
}