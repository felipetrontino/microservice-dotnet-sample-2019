using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Test.Mock.Bus
{
    public interface IBusMessageContainer
    {
        List<TMessage> GetAllSent<TMessage>(string queueName = null);

        List<TMessage> GetAllPublished<TMessage>(string exchangeName = null);

        Task AddQueue<T>(string contextName, T message, bool isSubscriber);

        bool HasAny();
    }
}