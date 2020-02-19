using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Bus
{
    public class BusContainer : IBusContainer
    {
        private readonly IBusPublisher _bus;
        private List<MessageCollection> _messages = new List<MessageCollection>();

        public BusContainer(IBusPublisher bus)
        {
            _bus = bus;
        }

        #region IBusPublisher

        Task IBusPublisher.PublishAsync<TMessage>(string exchangeName, IEnumerable<TMessage> messages)
        {
            _messages.Add(new MessageCollection(MessageType.Publish, exchangeName, null, messages));

            return Task.CompletedTask;
        }

        Task IBusPublisher.PublishAsync<TMessage>(string exchangeName, string topic, IEnumerable<TMessage> messages)
        {
            _messages.Add(new MessageCollection(MessageType.Topic, exchangeName, topic, messages));

            return Task.CompletedTask;
        }

        Task IBusPublisher.SendAsync<TMessage>(string queueName, IEnumerable<TMessage> messages)
        {
            _messages.Add(new MessageCollection(MessageType.Sended, queueName, null, messages));

            return Task.CompletedTask;
        }

        #endregion IBusPublisher

        public async Task CommitAsync()
        {
            var messages = _messages.ToList();
            _messages = new List<MessageCollection>();

            foreach (var message in messages)
            {
                switch (message.Type)
                {
                    case MessageType.Topic:
                        await _bus.PublishAsync(message.ContextName, message.Topic, message.Items);
                        break;

                    case MessageType.Sended:
                        await _bus.SendAsync(message.ContextName, message.Items);
                        break;

                    default:
                        await _bus.PublishAsync(message.ContextName, message.Items);
                        break;
                }
            }
        }

        public enum MessageType
        {
            Publish = 0,

            Topic = 1,

            Sended = 2
        }

        public class MessageCollection
        {
            public MessageCollection(MessageType type, string contextName, string topic, IEnumerable<IBusMessage> items)
            {
                Type = type;
                ContextName = contextName;
                Topic = topic;
                Items = items;
            }

            public MessageType Type { get; }

            public string ContextName { get; }

            public string Topic { get; }

            public IEnumerable<IBusMessage> Items { get; set; }
        }
    }
}