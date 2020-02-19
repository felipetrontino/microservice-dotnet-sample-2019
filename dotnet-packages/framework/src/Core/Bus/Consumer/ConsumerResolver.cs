using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Framework.Core.Bus.Consumer
{
    public class ConsumerResolver
    {
        public string ContentName { get; }

        public Type MessageType { get; }

        public string QueueName { get; }

        public string ExchangeName { get; }

        public string Topic { get; }

        public ConsumerResolver(string contentName, Type messageType, string queueName, string exchangeName, string topic = null)
        {
            ContentName = contentName;
            MessageType = messageType;
            QueueName = queueName;
            ExchangeName = exchangeName;
            Topic = topic;
        }

        public async Task ProcessAsync(IServiceProvider serviceProvider, object message)
        {
            var consumerType = typeof(IConsumer<>).MakeGenericType(MessageType);
            var consumer = serviceProvider.GetRequiredService(consumerType);

            var method = consumerType.GetMethod("Execute");
            await (Task)method.Invoke(consumer, new object[] { message });
        }
    }
}