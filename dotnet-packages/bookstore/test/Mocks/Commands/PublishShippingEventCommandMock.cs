using Bookstore.Domain.CommandSide.Commands;
using Framework.Test.Common;

namespace Bookstore.Tests.Mocks.Commands
{
    public class PublishShippingEventCommandMock : MockBuilder<PublishShippingEventCommandMock, PublishShippingEventCommand>
    {
        public static PublishShippingEventCommand Get(string key)
        {
            return Create(key).Default().Build();
        }

        public PublishShippingEventCommandMock Default()
        {
            Value.OrderId = MockBuilder.GetId(Key);

            return this;
        }
    }
}