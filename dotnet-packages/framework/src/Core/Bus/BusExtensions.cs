using System.Threading.Tasks;

namespace Framework.Core.Bus
{
    public static class BusExtensions
    {
        public static async Task PublishAsync<TMessage>(this IBusPublisher publisher, string exchangeName, TMessage message)
            where TMessage : class, IBusMessage
        {
            await publisher.PublishAsync(exchangeName, new[] { message });
        }

        public static async Task PublishAsync<TMessage>(this IBusPublisher publisher, string exchangeName, string topic, TMessage message)
            where TMessage : class, IBusMessage
        {
            await publisher.PublishAsync(exchangeName, topic, new[] { message });
        }

        public static async Task SendAsync<TMessage>(this IBusPublisher publisher, string queueName, TMessage message)
            where TMessage : class, IBusMessage
        {
            await publisher.SendAsync(queueName, new[] { message });
        }
    }
}