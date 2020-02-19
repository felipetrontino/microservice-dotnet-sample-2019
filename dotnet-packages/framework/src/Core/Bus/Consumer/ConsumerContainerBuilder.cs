using System.Collections.Generic;

namespace Framework.Core.Bus.Consumer
{
    public class ConsumerContainerBuilder : IConsumerContainerBuilder
    {
        private readonly List<ConsumerResolver> _consumers = new List<ConsumerResolver>();

        public static IConsumerContainerBuilder Create() => new ConsumerContainerBuilder();

        public IConsumerContainerBuilder Queue<TMessage>(string contentName, string queueName, string exchangeName = null, string topic = null)
            where TMessage : IBusMessage
        {
            var messageType = typeof(TMessage);
            var consumer = new ConsumerResolver(contentName, messageType, queueName, exchangeName, topic);

            _consumers.Add(consumer);

            return this;
        }

        public IConsumerContainer Build()
        {
            return new ConsumerContainer(_consumers);
        }
    }
}