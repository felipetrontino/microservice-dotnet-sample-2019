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
using Library.Domain.Entities;
using Library.Entities;
using Library.Infra.CrossCutting;
using Library.Tests.Mocks.Commands;
using Library.Tests.Mocks.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using Xunit;

namespace Library.Tests.ComponentTests.CommandSide
{
    [Collection(nameof(DatabaseFixture))]
    public class ReserveBookCommandHandlerTest
    {
        protected readonly IMockRepository<DbLibrary> MockRepository;
        protected readonly BusPublisherStub Bus;

        public ReserveBookCommandHandlerTest()
        {
            MockRepository = new EfMockRepository<DbLibrary>();
            Bus = BusPublisherStub.Create();
        }

        [Fact]
        public void Handle_ReserveBookCommandd_Default()
        {
            // arrange
            var key = MockBuilder.Key;

            var copy = CopyMock.Get(key);
            MockRepository.Add(copy);

            MockRepository.Commit();

            var command = ReserveBookCommandMock.Get(key);

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
            reservations.Should().BeEquivalentToEntity(MockBuilder.List(reservationExpected));

            var publishReservationEventCommands = Bus.GetAllSent<PublishReservationEventCommand>(ContextNames.Queue.Library);
            var PublishReservationEventCommandExpected = PublishReservationEventCommandMock.Get(key);
            PublishReservationEventCommandExpected.ReservationId = reservations[0].Id;
            publishReservationEventCommands.Should().BeEquivalentToMessage(MockBuilder.List(PublishReservationEventCommandExpected));
        }

        [Fact]
        public void Handle_ReserveBookCommand_Update()
        {
            // arrange
            var key = MockBuilder.Key;
            var key2 = MockBuilder.Key;

            var copy = CopyMock.Get(key2);
            MockRepository.Add(copy);

            var reservation = ReservationMock.Get(key);
            MockRepository.Add(reservation);

            MockRepository.Commit();

            var command = ReserveBookCommandMock.Get(key2);
            command.Number = reservation.Number;

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
            var reservationExpected = ReservationMock.Create(key)
                                        .BaseReservation()
                                        .WithItem(key2)
                                        .Build();
            reservationExpected.Number = reservation.Number;
            reservations.Should().BeEquivalentToEntity(MockBuilder.List(reservationExpected));

            var publishReservationEventCommands = Bus.GetAllSent<PublishReservationEventCommand>(ContextNames.Queue.Library);
            var PublishReservationEventCommandExpected = PublishReservationEventCommandMock.Get(key);
            PublishReservationEventCommandExpected.ReservationId = reservation.Id;
            publishReservationEventCommands.Should().BeEquivalentToMessage(MockBuilder.List(PublishReservationEventCommandExpected));
        }

        [Fact]
        public void Handle_ReserveBookCommand_Member_Exists()
        {
            // arrange
            var key = MockBuilder.Key;

            var copy = CopyMock.Get(key);
            MockRepository.Add(copy);

            var member = MemberMock.GetFull(key);
            MockRepository.Add(member);

            MockRepository.Commit();

            var command = ReserveBookCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var members = MockRepository.Query<Member>().ToList();
            members.Should().BeEquivalentToEntity(MockBuilder.List(member));

            var reservations = MockRepository.Query<Reservation>()
                                     .Include(x => x.Loans)
                                      .ThenInclude(x => x.Copy)
                                          .ThenInclude(x => x.Book)
                                     .Include(x => x.Member)
                                     .ToList();
            var reservationExpected = ReservationMock.Get(key);
            reservationExpected.Member = member;
            reservations.Should().BeEquivalentToEntity(MockBuilder.List(reservationExpected));
        }

        [Fact]
        public void Handle_ReserveBookCommand_Member_Not_Exists()
        {
            // arrange
            var key = MockBuilder.Key;

            var copy = CopyMock.Get(key);
            MockRepository.Add(copy);

            MockRepository.Commit();

            var command = ReserveBookCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var members = MockRepository.Query<Member>().ToList();
            var memberExpected = MemberMock.Get(key);
            members.Should().BeEquivalentToEntity(MockBuilder.List(memberExpected));

            var reservations = MockRepository.Query<Reservation>()
                                      .Include(x => x.Loans)
                                       .ThenInclude(x => x.Copy)
                                           .ThenInclude(x => x.Book)
                                      .Include(x => x.Member)
                                      .ToList();
            var reservationExpected = ReservationMock.Get(key);
            reservationExpected.Member = memberExpected;
            reservations.Should().BeEquivalentToEntity(MockBuilder.List(reservationExpected));
        }

        private bool Handle(ReserveBookCommand command)
        {
            var provider = DependencyInjectorStub.Get((s, c) =>
            {
                BootStrapper.RegisterServices(s, c);
                s.AddScoped(x => MockRepository.GetContext());
                s.AddScoped<IBusPublisher>(x => Bus);
                s.AddScoped<IDateTimeService>(x => DateTimeServiceStub.Create());
            });

            var handler = provider.GetRequiredService<IRequestHandler<ReserveBookCommand, bool>>();
            return handler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}