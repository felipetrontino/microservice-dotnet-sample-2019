using Framework.Test.Common;
using Library.Entities;
using Library.Tests.Utils;

namespace Library.Tests.Mocks.Entities
{
    public class ReservationMock : MockBuilder<ReservationMock, Reservation>
    {
        public static Reservation Get(string key)
        {
            return Create(key).Default().Build();
        }

        public ReservationMock Default()
        {
            BaseReservation();

            WithItem(Key);

            return this;
        }

        public ReservationMock BaseReservation()
        {
            Value.Number = Fake.GetReservationNumber(Key);
            Value.Status = Fake.GetStatusReservation();
            Value.RequestDate = Fake.GetRequestDate();
            Value.Member = MemberMock.Get(Key);

            return this;
        }

        public ReservationMock WithItem(string itemKey = null)
        {
            Value.Loans.Add(LoanMock.Get(itemKey));

            return this;
        }
    }
}