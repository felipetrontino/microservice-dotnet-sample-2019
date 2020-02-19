using FluentAssertions;
using Framework.Core.Bus;
using Framework.Test.Common;
using Framework.Test.Data;
using Framework.Test.Extensions;
using Framework.Test.Mock.Bus;
using Library.Data;
using Library.Domain.CommandSide.Commands;
using Library.Domain.Common;
using Library.Domain.Models.Event;
using Library.Infra.CrossCutting;
using Library.Tests.Mocks.Commands;
using Library.Tests.Mocks.Entities;
using Library.Tests.Mocks.Models.Event;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Xunit;

namespace Library.Tests.ComponentTests.CommandSide
{
    [Collection(nameof(DatabaseFixture))]
    public class PublishEventCommandHandlerTest
    {
        protected readonly IMockRepository<DbLibrary> MockRepository;
        protected readonly BusPublisherStub Bus;

        public PublishEventCommandHandlerTest()
        {
            MockRepository = new EfMockRepository<DbLibrary>();
            Bus = BusPublisherStub.Create();
        }

        [Fact]
        public void Handle_PublishReservationEventCommandd_Default()
        {
            // arrange
            var key = MockBuilder.Key;

            var reservation = ReservationMock.Get(key);
            MockRepository.Add(reservation);

            MockRepository.Commit();

            var command = PublishReservationEventCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var events = Bus.GetAllPublished<CreateReservationEvent>(ContextNames.Exchange.Library);
            var eventExpected = CreateReservationEventMock.Get(key);
            events.Should().BeEquivalentToMessage(MockBuilder.List(eventExpected));
        }

        private bool Handle(PublishReservationEventCommand command)
        {
            var provider = DependencyInjectorStub.Get((s, c) =>
            {
                BootStrapper.RegisterServices(s, c);
                s.AddScoped(x => MockRepository.GetContext());
                s.AddScoped<IBusPublisher>(x => Bus);
            });
            var handler = provider.GetRequiredService<IRequestHandler<PublishReservationEventCommand, bool>>();
            return handler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}