using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Test.Mock.Bus
{
    public class MediatorStub : IMediator, IBusMessageContainer
    {
        public static MediatorStub Create(BusMessageContainer container = null) => new MediatorStub(container);

        public MediatorStub(BusMessageContainer container = null)
        {
            Container = container ?? new BusMessageContainer();
        }

        protected BusMessageContainer Container { get; }

        public async Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            await Container.AddQueue(request.GetType().Name, request, false);

            return default;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
        {
            await Container.AddQueue(request.GetType().Name, request, false);

            return default;
        }

        public async Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
        {
            await Container.AddQueue(notification.GetType().Name, notification, true);
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken())
            where TNotification : INotification
        {
            await Container.AddQueue(notification.GetType().Name, notification, true);
        }

        public List<TMessage> GetAllSent<TMessage>(string queueName = null)
        {
            return Container.GetAllSent<TMessage>(queueName);
        }

        public List<TMessage> GetAllPublished<TMessage>(string exchangeName = null)
        {
            return Container.GetAllPublished<TMessage>(exchangeName);
        }

        public Task AddQueue<TMessage>(string contextName, TMessage message, bool isSubscriber)
        {
            return Container.AddQueue<TMessage>(contextName, message, isSubscriber);
        }

        public bool HasAny()
        {
            return Container.HasAny();
        }
    }
}