using System;
using System.Threading.Tasks;

namespace Framework.Core.Bus
{
    public interface IBusClient
    {
        Task ReceiveAsync<TMessage>(string queueName, Func<TMessage, Task> funcAsync) where TMessage : class;

        Task ReceiveAsync<TMessage>(string queueName, string exchangeName, Func<TMessage, Task> funcAsync) where TMessage : class;
    }
}