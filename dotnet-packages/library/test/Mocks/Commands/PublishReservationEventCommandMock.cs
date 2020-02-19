using Framework.Test.Common;
using Library.Domain.CommandSide.Commands;

namespace Library.Tests.Mocks.Commands
{
    public class PublishReservationEventCommandMock : MockBuilder<PublishReservationEventCommandMock, PublishReservationEventCommand>
    {
        public static PublishReservationEventCommand Get(string key)
        {
            return Create(key).Default().Build();
        }

        public PublishReservationEventCommandMock Default()
        {
            Value.ReservationId = MockBuilder.GetId(Key);

            return this;
        }
    }
}