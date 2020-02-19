using Framework.Test.Common;
using Library.Domain.CommandSide.Commands;

namespace Library.Tests.Mocks.Commands
{
    public class ExpireReservationCommandMock : MockBuilder<ExpireReservationCommandMock, ExpireReservationCommand>
    {
        public static ExpireReservationCommand Get(string key)
        {
            return Create(key).Default().Build();
        }

        public ExpireReservationCommandMock Default()
        {
            Value.ReservationId = MockBuilder.GetId(Key);

            return this;
        }
    }
}