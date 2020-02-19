using Framework.Core.Bus;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Test.Mock.Bus
{
    public class BusPublisherStub : IBusPublisher, IBusMessageContainer
    {
        public static BusPublisherStub Create(BusMessageContainer container = null) => new BusPublisherStub(container);

        private readonly Resolver _messageResolver = new Resolver();
        private IMediator _mediator;

        public BusPublisherStub(BusMessageContainer container = null)
        {
            Container = container ?? new BusMessageContainer();
        }

        protected BusMessageContainer Container { get; }

        #region IBusPublisher

        public async Task PublishAsync<TMessage>(string exchangeName, IEnumerable<TMessage> messages)
            where TMessage : class, IBusMessage
        {
            await ProcessAsync(exchangeName, null, messages, true);
        }

        public async Task PublishAsync<TMessage>(string exchangeName, string topic, IEnumerable<TMessage> messages)
            where TMessage : class, IBusMessage
        {
            await ProcessAsync(exchangeName, topic, messages, true);
        }

        public async Task SendAsync<TMessage>(string queueName, IEnumerable<TMessage> messages)
            where TMessage : class, IBusMessage
        {
            await ProcessAsync(queueName, null, messages, false);
        }

        #endregion IBusPublisher

        #region IBusMessageContainer

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

        #endregion IBusMessageContainer

        public void IntegrateByMediator(IMediator mediator, Action<Resolver> mapper)
        {
            _mediator = mediator;
            mapper?.Invoke(_messageResolver);
        }

        private async Task ProcessAsync<TMessage>(string exchangeNameOrQueueName, string topic,
            IEnumerable<TMessage> messages, bool isSubscriber) where TMessage : class, IBusMessage
        {
            var contextName = GetContextName(exchangeNameOrQueueName, topic, isSubscriber);
            var destinationType = _messageResolver.GetTypes(contextName);

            foreach (var message in messages)
            {
                await Container.AddQueue(exchangeNameOrQueueName, message, isSubscriber);
                await ExecuteAsync(message, destinationType);
            }
        }

        private async Task ExecuteAsync<TMessage>(TMessage message, Resolver.DestinationTypes destinationType)
            where TMessage : class, IBusMessage
        {
            if (destinationType == null) return;

            var originSerialized = JsonConvert.SerializeObject(message);

            dynamic destination = JsonConvert.DeserializeObject(originSerialized, destinationType.Request);

            if (_mediator == null) return;

            await _mediator.Send(destination);
        }

        private static string GetContextName(string exchangeNameOrQueueName, string topic, bool isSubscriber)
        {
            return $"{exchangeNameOrQueueName}_{topic}_{isSubscriber}";
        }

        public class Resolver
        {
            private readonly Dictionary<string, DestinationTypes> _types = new Dictionary<string, DestinationTypes>();

            [Obsolete("Use LinkQueue instead of.")]
            public void Add<THandler, TCommand>(string exchangeNameOrQueueName = null, string topic = null)
            {
                LinkQueue<THandler, TCommand>(exchangeNameOrQueueName);
            }

            public void LinkQueue<THandler, TCommand>(string queueName = null)
            {
                var contextName = GetContextName(queueName, null, false);
                Add<THandler, TCommand>(contextName);
            }

            public void LinkExchangeQueue<THandler, TCommand>(string exchangeName = null)
            {
                var contextName = GetContextName(exchangeName, null, true);
                Add<THandler, TCommand>(contextName);
            }

            public void LinkTopicExchange<THandler, TCommand>(string exchangeName = null, string topic = null)
            {
                var contextName = GetContextName(exchangeName, topic, true);
                Add<THandler, TCommand>(contextName);
            }

            private void Add<THandler, TCommand>(string contextName)
            {
                var handlerType = typeof(THandler);
                var commandType = typeof(TCommand);

                var interfaces = handlerType.GetInterfaces();

                foreach (var interfaceType in interfaces)
                {
                    if (!interfaceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IRequestHandler<,>)))
                        continue;

                    var handlerCommandType = interfaceType.GetGenericArguments()[0];

                    if (handlerCommandType != commandType) continue;

                    var handlerResponseType = interfaceType.GetGenericArguments()[1];

                    _types[contextName] = new DestinationTypes(handlerCommandType, handlerResponseType);
                }
            }

            public DestinationTypes GetTypes(string contextName)
            {
                return _types.GetValueOrDefault(contextName);
            }

            public class DestinationTypes
            {
                public DestinationTypes(Type request, Type response)
                {
                    Request = request;
                    Response = response;
                }

                public Type Request { get; }

                public Type Response { get; }
            }
        }
    }
}