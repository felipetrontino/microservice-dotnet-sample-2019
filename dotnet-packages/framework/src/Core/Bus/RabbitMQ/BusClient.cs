using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Framework.Core.Bus.RabbitMQ
{
    public class BusClient : IBusClient
    {
        private readonly IBusReceiver _busReceiver;

        public BusClient(string connectionString)
        {
            var conn = new BusConnection(connectionString);
            _busReceiver = new BusReceiver(conn);
        }

        public async Task ReceiveAsync<TMessage>(string queueName, Func<TMessage, Task> funcAsync)
            where TMessage : class
        {
            await _busReceiver.ReceiveAsync(queueName, null, null, async x =>
            {
                var message = JsonConvert.DeserializeObject(x.Body, typeof(TMessage)) as TMessage;
                await funcAsync(message);
            });
        }

        public async Task ReceiveAsync<TMessage>(string queueName, string exchangeName, Func<TMessage, Task> funcAsync)
            where TMessage : class
        {
            await _busReceiver.ReceiveAsync(queueName, exchangeName, null, async x =>
           {
               var message = JsonConvert.DeserializeObject(x.Body, typeof(TMessage)) as TMessage;
               await funcAsync(message);
           });
        }
    }
}