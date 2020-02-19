﻿//using FluentAssertions;
//using Framework.Test.Common;
//using Framework.Test.Data;
//using Framework.Test.Mock.Bus;
//using Library.Core.Common;
//using Library.Core.Enums;
//using Library.Data;
//using Library.Models.Message;
//using Library.Services;
//using Library.Tests.Mocks.Entities;
//using Library.Tests.Mocks.Models.Message;
//using Library.Tests.Utils;
//using System;
//using System.Linq;
//using Xunit;

//namespace Library.Tests.Services
//{
//    public class ReservationServiceTest : BaseTest
//    {
//        protected DbLibrary Db { get; }
//        protected IMockRepository MockRepository { get; }

//        public ReservationServiceTest()
//        {
//            Db = MockHelper.GetDbContext<DbLibrary>();
//            MockRepository = new EFMockRepository(Db);
//        }

//        #region RequestAsync

//        [Fact]
//        public void RequestAsync_Reservation_Insert_Valid()
//        {
//            var key = FakeHelper.Key;
//            var book = BookMock.Get(key);
//            var copy = CopyMock.Get(key);

//            MockRepository.Add(book);
//            MockRepository.Add(copy);

//            var message = ReservationMessageMock.Get(key);

//            var dto = RequestAsync(message);
//            dto.Should().NotBeNull();

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.GetByReservation(key);
//            expected.Id = entity.Id;
//            expected.Member.Id = entity.Member.Id;
//            expected.Loans[0].Id = entity.Loans[0].Id;
//            entity.Should().BeEquivalentToEntity(expected);

//            var dtoExpected = ReservationEventMessageMock.Get(key);
//            dtoExpected.ReservationId = entity.Id;
//            dto.Should().BeEquivalentToMessage(dtoExpected);
//        }

//        [Fact]
//        public void RequestAsync_Reservation_Insert_BookNotExists()
//        {
//            var key = FakeHelper.Key;

//            var message = ReservationMessageMock.Get(key);

//            var dto = RequestAsync(message);
//            dto.Should().NotBeNull();

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.GetByReservation(key);
//            expected.Id = entity.Id;
//            expected.Member.Id = entity.Member.Id;
//            expected.Loans[0].Id = entity.Loans[0].Id;
//            expected.Loans[0].Book = null;
//            expected.Loans[0].Copy = null;
//            entity.Should().BeEquivalentToEntity(expected);

//            var dtoExpected = ReservationEventMessageMock.Get(key);
//            dtoExpected.ReservationId = entity.Id;
//            dto.Should().BeEquivalentToMessage(dtoExpected);
//        }

//        [Fact]
//        public void RequestAsync_Reservation_Insert_CopyNotExists()
//        {
//            var key = FakeHelper.Key;
//            var book = BookMock.Get(key);

//            MockRepository.Add(book);

//            var message = ReservationMessageMock.Get(key);

//            var dto = RequestAsync(message);
//            dto.Should().NotBeNull();

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.GetByReservation(key);
//            expected.Id = entity.Id;
//            expected.Member.Id = entity.Member.Id;
//            expected.Loans[0].Id = entity.Loans[0].Id;
//            expected.Loans[0].Copy = null;
//            entity.Should().BeEquivalentToEntity(expected);

//            var dtoExpected = ReservationEventMessageMock.Get(key);
//            dtoExpected.ReservationId = entity.Id;
//            dto.Should().BeEquivalentToMessage(dtoExpected);
//        }

//        [Fact]
//        public void RequestAsync_Reservation_Insert_WithoutCopyNumber()
//        {
//            var key = FakeHelper.Key;
//            var book = BookMock.Get(key);
//            var copy = CopyMock.Get(key);

//            MockRepository.Add(book);
//            MockRepository.Add(copy);

//            var message = ReservationMessageMock.Get(key);
//            message.Items[0].Number = null;

//            var dto = RequestAsync(message);
//            dto.Should().NotBeNull();

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.GetByReservation(key);
//            expected.Id = entity.Id;
//            expected.Member.Id = entity.Member.Id;
//            expected.Loans[0].Id = entity.Loans[0].Id;
//            expected.Loans[0].Copy = null;
//            entity.Should().BeEquivalentToEntity(expected);

//            var dtoExpected = ReservationEventMessageMock.Get(key);
//            dtoExpected.ReservationId = entity.Id;
//            dto.Should().BeEquivalentToMessage(dtoExpected);
//        }

//        [Fact]
//        public void RequestAsync_Reservation_Update_Valid()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.GetByReservation(key);

//            MockRepository.Add(reservation);

//            var message = ReservationMessageMock.Get(key);

//            var dto = RequestAsync(message);
//            dto.Should().NotBeNull();

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.GetByReservation(key);
//            expected.UpdatedAt = DateTime.UtcNow;
//            expected.Member.Id = entity.Member.Id;
//            expected.Loans[0].Id = entity.Loans[0].Id;
//            entity.Should().BeEquivalentToEntity(expected);

//            var dtoExpected = ReservationEventMessageMock.Get(key);
//            dto.Should().BeEquivalentToMessage(dtoExpected);
//        }

//        [Fact]
//        public void RequestAsync_Reservation_Update_Member_NotAccepted()
//        {
//            var key = FakeHelper.Key;
//            var order = ReservationMock.GetByReservation(key);
//            MockRepository.Add(order);

//            var message = ReservationMessageMock.Get(key);
//            var key2 = FakeHelper.Key;
//            message.MemberName = Fake.GetMemberName(key2);
//            message.MemberId = FakeHelper.GetId(key2).ToString();

//            var dto = RequestAsync(message);
//            dto.Should().NotBeNull();

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.GetByReservation(key);
//            expected.UpdatedAt = DateTime.UtcNow;
//            expected.Loans[0].Id = entity.Loans[0].Id;
//            expected.Member.Id = entity.Member.Id;

//            entity.Should().BeEquivalentToEntity(expected);

//            var dtoExpected = ReservationEventMessageMock.Get(key);
//            dto.Should().BeEquivalentToMessage(dtoExpected);
//        }

//        #endregion RequestAsync

//        #region ReturnAsync

//        [Fact]
//        public void ReturnAsync_Reservation_Valid()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);

//            MockRepository.Add(reservation);

//            var message = ReservationReturnMessageMock.Get(key);

//            ReturnAsync(message);

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.Get(key);
//            expected.Status = StatusReservation.Deliveried;
//            expected.Loans[0].ReturnDate = Fake.GetReturnDate(key);
//            entity.Should().BeEquivalentToEntity(expected);
//        }

//        [Fact]
//        public void ReturnAsync_Reservation_StatusDeliveried()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            reservation.Status = StatusReservation.Deliveried;

//            MockRepository.Add(reservation);

//            var message = ReservationReturnMessageMock.Get(key);

//            ReturnAsync(message);

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.Get(key);
//            expected.Status = StatusReservation.Deliveried;
//            expected.Loans[0].ReturnDate = Fake.GetReturnDate(key);
//            entity.Should().BeEquivalentToEntity(expected);
//        }

//        [Fact]
//        public void ReturnAsync_Reservation_StatusCancelled()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            reservation.Status = StatusReservation.Cancelled;
//            MockRepository.Add(reservation);

//            var message = ReservationReturnMessageMock.Get(key);

//            ReturnAsync(message);

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.Get(key);
//            expected.Status = StatusReservation.Deliveried;
//            expected.Loans[0].ReturnDate = Fake.GetReturnDate(key);
//            entity.Should().BeEquivalentToEntity(expected);
//        }

//        [Fact]
//        public void ReturnAsync_Reservation_StatusExpired()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            reservation.Status = StatusReservation.Expired;
//            MockRepository.Add(reservation);

//            var message = ReservationReturnMessageMock.Get(key);

//            ReturnAsync(message);

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.Get(key);
//            expected.Status = StatusReservation.Deliveried;
//            expected.Loans[0].ReturnDate = reservation.Loans[0].ReturnDate;
//            entity.Should().BeEquivalentToEntity(expected);
//        }

//        #endregion ReturnAsync

//        #region CheckDueAsync

//        [Fact]
//        public void CheckDueAsync_Reservation_Valid()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            reservation.Loans[0].DueDate = DateTime.UtcNow;

//            MockRepository.Add(reservation);

//            var dto = CheckDueAsync();
//            dto.Should().NotBeNull();

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));
//            var expected = ReservationMock.Get(key);
//            expected.Loans[0].DueDate = reservation.Loans[0].DueDate;
//            entity.Should().BeEquivalentToEntity(expected);

//            var dtoExpected = ReservationExpiredMessageMock.Get(key);
//            dto.Should().BeEquivalentToMessage(dtoExpected);
//        }

//        [Fact]
//        public void CheckDueAsync_Reservation_DueDate_Minor()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            reservation.Loans[0].DueDate = DateTime.UtcNow.Date.AddDays(1).AddMinutes(-1);

//            MockRepository.Add(reservation);

//            var dto = CheckDueAsync();
//            dto.Should().NotBeNull();

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));
//            var expected = ReservationMock.Get(key);
//            expected.Loans[0].DueDate = reservation.Loans[0].DueDate;
//            entity.Should().BeEquivalentToEntity(expected);

//            var dtoExpected = ReservationExpiredMessageMock.Get(key);
//            dto.Should().BeEquivalentToMessage(dtoExpected);
//        }

//        [Fact]
//        public void CheckDueAsync_Reservation_DueDate_Equals()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            reservation.Loans[0].DueDate = DateTime.UtcNow.Date.AddDays(1);

//            MockRepository.Add(reservation);

//            var dto = CheckDueAsync();
//            dto.Should().BeNull();

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));
//            var expected = ReservationMock.Get(key);
//            expected.Loans[0].DueDate = reservation.Loans[0].DueDate;
//            entity.Should().BeEquivalentToEntity(expected);
//        }

//        [Fact]
//        public void CheckDueAsync_Reservation_DueDate_Major()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            reservation.Loans[0].DueDate = DateTime.UtcNow.Date.AddDays(1).AddMinutes(1);

//            MockRepository.Add(reservation);

//            var dto = CheckDueAsync();
//            dto.Should().BeNull();

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));
//            var expected = ReservationMock.Get(key);
//            expected.Loans[0].DueDate = reservation.Loans[0].DueDate;
//            entity.Should().BeEquivalentToEntity(expected);
//        }

//        #endregion CheckDueAsync

//        #region ExpireAsync

//        [Fact]
//        public void ExpireAsync_Reservation_Valid()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            MockRepository.Add(reservation);

//            var message = ReservationExpiredMessageMock.Get(key);

//            ExpireAsync(message);

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.Get(key);
//            expected.Status = StatusReservation.Expired;
//            entity.Should().BeEquivalentToEntity(expected);
//        }

//        [Fact]
//        public void ExpireAsync_Reservation_StatusExpired()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            reservation.Status = StatusReservation.Expired;
//            MockRepository.Add(reservation);

//            var message = ReservationExpiredMessageMock.Get(key);

//            ExpireAsync(message);

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.Get(key);
//            expected.Status = StatusReservation.Expired;
//            entity.Should().BeEquivalentToEntity(expected);
//        }

//        [Fact]
//        public void ExpireAsync_Reservation_StatusCancelled()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            reservation.Status = StatusReservation.Cancelled;
//            MockRepository.Add(reservation);

//            var message = ReservationExpiredMessageMock.Get(key);

//            ExpireAsync(message);

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.Get(key);
//            expected.Status = StatusReservation.Expired;
//            entity.Should().BeEquivalentToEntity(expected);
//        }

//        [Fact]
//        public void ExpireAsync_Reservation_StatusDeliveried()
//        {
//            var key = FakeHelper.Key;
//            var reservation = ReservationMock.Get(key);
//            reservation.Status = StatusReservation.Deliveried;
//            MockRepository.Add(reservation);

//            var message = ReservationExpiredMessageMock.Get(key);

//            ExpireAsync(message);

//            var entity = Db.Reservations.FirstOrDefault(x => x.Number == Fake.GetReservationNumber(key));

//            var expected = ReservationMock.Get(key);
//            expected.Status = StatusReservation.Expired;
//            entity.Should().BeEquivalentToEntity(expected);
//        }

//        #endregion ExpireAsync

//        #region Utils

//        private ReservationEventMessage RequestAsync(ReservationMessage message)
//        {
//            var bus = BusPublisherStub.Create();

//            var service = new ReservationService(Db, bus);
//            service.RequestAsync(message).Wait();

//            return bus.Dequeue<ReservationEventMessage>(QueueNames.Library);
//        }

//        private void ReturnAsync(ReservationReturnMessage message)
//        {
//            var service = new ReservationService(Db, BusPublisherStub.Create());
//            service.ReturnAsync(message).Wait();
//        }

//        private ReservationExpiredMessage CheckDueAsync()
//        {
//            var bus = BusPublisherStub.Create();

//            var service = new ReservationService(Db, bus);
//            service.CheckDueAsync().Wait();

//            return bus.Dequeue<ReservationExpiredMessage>(QueueNames.Library);
//        }

//        private void ExpireAsync(ReservationExpiredMessage message)
//        {
//            var bus = BusPublisherStub.Create();

//            var service = new ReservationService(Db, bus);
//            service.ExpireAsync(message).Wait();
//        }

//        #endregion Utils
//    }
//}