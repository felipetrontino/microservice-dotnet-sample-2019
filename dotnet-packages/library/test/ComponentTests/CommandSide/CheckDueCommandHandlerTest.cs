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
using Library.Infra.CrossCutting;
using Library.Tests.Mocks.Commands;
using Library.Tests.Mocks.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Xunit;

namespace Library.Tests.ComponentTests.CommandSide
{
    [Collection(nameof(DatabaseFixture))]
    public class CheckDueCommandHandlerTest
    {
        protected readonly IMockRepository<DbLibrary> MockRepository;
        protected readonly BusPublisherStub Bus;

        public CheckDueCommandHandlerTest()
        {
            MockRepository = new EfMockRepository<DbLibrary>();
            Bus = BusPublisherStub.Create();
        }

        [Fact]
        public void Handle_CheckDueCommand_Default()
        {
            // arrange
            var key = MockBuilder.Key;

            var reservation = ReservationMock.Get(key);

            MockRepository.Add(reservation);

            MockRepository.Commit();

            var command = CheckDueCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var expireReservationCommands = Bus.GetAllSent<ExpireReservationCommand>(ContextNames.Queue.Library);
            expireReservationCommands.Should().BeEmpty();
        }

        [Fact]
        public void Handle_CheckDueCommand_Reservation_Expire_Today()
        {
            // arrange
            var key = MockBuilder.Key;

            var reservation = ReservationMock.Get(key);
            reservation.Loans[0].DueDate = MockBuilder.Date;

            MockRepository.Add(reservation);

            MockRepository.Commit();

            var command = CheckDueCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var expireReservationCommands = Bus.GetAllSent<ExpireReservationCommand>(ContextNames.Queue.Library);
            var expireReservationCommandExpected = ExpireReservationCommandMock.Get(key);
            expireReservationCommands.Should().BeEquivalentToMessage(MockBuilder.List(expireReservationCommandExpected));
        }

        [Fact]
        public void Handle_CheckDueCommand_Reservation_Expire_Yesterday()
        {
            // arrange
            var key = MockBuilder.Key;

            var reservation = ReservationMock.Get(key);
            reservation.Loans[0].DueDate = MockBuilder.Date.AddDays(-1);

            MockRepository.Add(reservation);

            MockRepository.Commit();

            var command = CheckDueCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var expireReservationCommands = Bus.GetAllSent<ExpireReservationCommand>(ContextNames.Queue.Library);
            var expireReservationCommandExpected = ExpireReservationCommandMock.Get(key);
            expireReservationCommands.Should().BeEquivalentToMessage(MockBuilder.List(expireReservationCommandExpected));
        }

        [Fact]
        public void Handle_CheckDueCommand_Reservation_Expire_Tomorrow()
        {
            // arrange
            var key = MockBuilder.Key;

            var reservation = ReservationMock.Get(key);
            reservation.Loans[0].DueDate = MockBuilder.Date.AddDays(1);

            MockRepository.Add(reservation);

            MockRepository.Commit();

            var command = CheckDueCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var expireReservationCommands = Bus.GetAllSent<ExpireReservationCommand>(ContextNames.Queue.Library);
            expireReservationCommands.Should().BeEmpty();
        }

        private bool Handle(CheckDueCommand command)
        {
            var provider = DependencyInjectorStub.Get((s, c) =>
            {
                BootStrapper.RegisterServices(s, c);
                s.AddScoped(x => MockRepository.GetContext());
                s.AddScoped<IBusPublisher>(x => Bus);
                s.AddScoped<IDateTimeService>(x => DateTimeServiceStub.Create());
            });

            var handler = provider.GetRequiredService<IRequestHandler<CheckDueCommand, bool>>();
            return handler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}