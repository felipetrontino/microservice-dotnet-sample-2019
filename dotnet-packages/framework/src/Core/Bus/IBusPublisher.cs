using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.Bus
{
    public interface IBusPublisher
    {
        Task PublishAsync<TMessage>(string exchangeName, IEnumerable<TMessage> messages) where TMessage : class, IBusMessage;

        Task PublishAsync<TMessage>(string exchangeName, string topic, IEnumerable<TMessage> messages) where TMessage : class, IBusMessage;

        Task SendAsync<TMessage>(string queueName, IEnumerable<TMessage> messages) where TMessage : class, IBusMessage;
    }
}