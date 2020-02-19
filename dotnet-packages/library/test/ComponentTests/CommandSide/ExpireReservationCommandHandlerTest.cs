using FluentAssertions;
using Framework.Core.Bus;
using Framework.Core.Common;
using Framework.Test.Common;
using Framework.Test.Data;
using Framework.Test.Extensions;
using Framework.Test.Mock.Bus;
using Framework.Test.Mock.Common;
using Library.Data;
using Library.Domain.CommandSide.Commands;
using Library.Domain.Common;
using Library.Domain.Enums;
using Library.Entities;
using Library.Infra.CrossCutting;
using Library.Tests.Mocks.Commands;
using Library.Tests.Mocks.Entities;
using Library.Tests.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using Xunit;

namespace Library.Tests.ComponentTests.CommandSide
{
    [Collection(nameof(DatabaseFixture))]
    public class ExpireReservationCommandHandlerTest
    {
        protected readonly IMockRepository<DbLibrary> MockRepository;
        protected readonly BusPublisherStub Bus;

        public ExpireReservationCommandHandlerTest()
        {
            MockRepository = new EfMockRepository<DbLibrary>();
            Bus = BusPublisherStub.Create();
        }

        [Fact]
        public void Handle_ExpireReservationCommand_Default()
        {
            // arrange
            var key = MockBuilder.Key;

            var reservation = ReservationMock.Get(key);

            MockRepository.Add(reservation);

            MockRepository.Commit();

            var command = ExpireReservationCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var reservations = MockRepository.Query<Reservation>()
                               .Include(x => x.Loans)
                                .ThenInclude(x => x.Copy)
                                    .ThenInclude(x => x.Book)
                               .Include(x => x.Member)
                               .ToList();
            var reservationExpected = ReservationMock.Get(key);
            reservationExpected.Status = StatusReservation.Expired;
            reservations.Should().BeEquivalentToEntity(MockBuilder.List(reservationExpected));

            var publishReservationEventCommands = Bus.GetAllSent<PublishReservationEventCommand>(ContextNames.Queue.Library);
            var publishReservationEventCommandExpected = PublishReservationEventCommandMock.Get(key);
            publishReservationEventCommands.Should().BeEquivalentToMessage(MockBuilder.List(publishReservationEventCommandExpected));
        }

        private bool Handle(ExpireReservationCommand command)
        {
            var provider = DependencyInjectorStub.Get((s, c) =>
            {
                BootStrapper.RegisterServices(s, c);
                s.AddScoped(x => MockRepository.GetContext());
                s.AddScoped<IBusPublisher>(x => Bus);
            });

            var handler = provider.GetRequiredService<IRequestHandler<ExpireReservationCommand, bool>>();
            return handler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}