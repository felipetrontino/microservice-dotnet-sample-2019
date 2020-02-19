using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Test.Mock.Bus
{
    public class BusMessageContainer : IBusMessageContainer
    {
        private Dictionary<string, List<object>> Queues { get; } = new Dictionary<string, List<object>>();

        public List<TMessage> GetAllSent<TMessage>(string queueName = null)
        {
            return ReadQueue<TMessage>(queueName, false);
        }

        public List<TMessage> GetAllPublished<TMessage>(string exchangeName = null)
        {
            return ReadQueue<TMessage>(exchangeName, true);
        }

        public async Task AddQueue<TMessage>(string contextName, TMessage message, bool isSubscriber)
        {
            var name = $"{contextName}_{isSubscriber}";
            await Task.Run(() => { Enqueue(name, message); });
        }

        private List<TMessage> ReadQueue<TMessage>(string contextName, bool isSubscriber)
        {
            contextName ??= typeof(TMessage).Name;

            var name = $"{contextName}_{isSubscriber}";

            var queue = GetQueue(name);
            var list = queue.OfType<TMessage>().ToList();

            list.ForEach(x => queue.Remove(x));

            return list;
        }

        private void Enqueue(string contextName, object message)
        {
            var queue = GetQueue(contextName);
            queue.Add(message);
        }

        private List<object> GetQueue(string contextName)
        {
            var queue = Queues.FirstOrDefault(x => x.Key == contextName).Value;
            if (queue != null) return queue;

            queue = new List<object>();
            Queues.Add(contextName, queue);

            return queue;
        }

        public bool HasAny()
        {
            return Queues.Any(x => x.Value.Any());
        }
    }
}