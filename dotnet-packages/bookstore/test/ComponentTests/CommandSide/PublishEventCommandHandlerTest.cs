using Bookstore.Data;
using Bookstore.Domain.CommandSide.Commands;
using Bookstore.Domain.Common;
using Bookstore.Domain.Models.Event;
using Bookstore.Infra.CrossCutting;
using Bookstore.Tests.Mocks.Commands;
using Bookstore.Tests.Mocks.Entities;
using Bookstore.Tests.Mocks.Models.Event;
using FluentAssertions;
using Framework.Core.Bus;
using Framework.Test.Common;
using Framework.Test.Data;
using Framework.Test.Extensions;
using Framework.Test.Mock.Bus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Xunit;

namespace Bookstore.Tests.ComponentTests.CommandSide
{
    [Collection(nameof(DatabaseFixture))]
    public class PublishEventCommandHandlerTest
    {
        protected readonly IMockRepository<DbBookstore> MockRepository;
        protected readonly BusPublisherStub Bus;

        public PublishEventCommandHandlerTest()
        {
            MockRepository = new EfMockRepository<DbBookstore>();
            Bus = BusPublisherStub.Create();
        }

        [Fact]
        public void Handle_PublishShippingEventCommand_Default()
        {
            // arrange
            var key = MockBuilder.Key;

            var order = OrderMock.Get(key);
            MockRepository.Add(order);

            MockRepository.Commit();

            var command = PublishShippingEventCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var events = Bus.GetAllPublished<CreateShippingEvent>(ContextNames.Exchange.Bookstore);
            var eventExpected = CreateShippingEventMock.Get(key);
            events.Should().BeEquivalentToMessage(MockBuilder.List(eventExpected));
        }

        private bool Handle(PublishShippingEventCommand command)
        {
            var provider = DependencyInjectorStub.Get((s, c) =>
            {
                BootStrapper.RegisterServices(s, c);
                s.AddScoped(x => MockRepository.GetContext());
                s.AddScoped<IBusPublisher>(x => Bus);
            });

            var handler = provider.GetRequiredService<IRequestHandler<PublishShippingEventCommand, bool>>();
            return handler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}