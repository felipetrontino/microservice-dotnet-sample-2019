using Framework.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Bus.Consumer
{
    public class ConsumerHandler : IConsumerHandler
    {
        private readonly IBusReceiver _bus;
        private readonly IServiceProvider _serviceProvider;

        public ConsumerHandler(IBusReceiver bus, IServiceProvider serviceProvider)
        {
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        protected IConsumerContainer Container { get; private set; }

        public void SetContainer(IConsumerContainer container)
        {
            Container = container;
        }

        public async Task StartAsync()
        {
            var consumers = Container.GetAll();

            var groups = consumers.GroupBy(x => new { x.QueueName, x.ExchangeName, x.Topic });

            foreach (var group in groups)
            {
                var groupConsumers = group.ToList();

                await _bus.ReceiveAsync(group.Key.QueueName, group.Key.ExchangeName, group.Key.Topic, async (message) =>
                {
                    var consumer = groupConsumers.FirstOrDefault(x => x.ContentName == message.ContentName);

                    if (consumer == null) throw new BusException("Consumer not found");

                    var messageData = JsonConvert.DeserializeObject(message.Body, consumer.MessageType);

                    using var scope = _serviceProvider.CreateScope();
                    await consumer.ProcessAsync(scope.ServiceProvider, messageData);
                });
            }
        }
    }
}