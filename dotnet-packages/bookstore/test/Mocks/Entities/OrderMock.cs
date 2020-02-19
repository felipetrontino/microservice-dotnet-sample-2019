using Bookstore.Domain.Entities;
using Bookstore.Tests.Utils;
using Framework.Test.Common;

namespace Bookstore.Tests.Mocks.Entities
{
    public class OrderMock : MockBuilder<OrderMock, Order>
    {
        public static Order Get(string key)
        {
            return Create(key).Default().Build();
        }

        public OrderMock Default()
        {
            BaseOrder();
            WithItem(Key);

            return this;
        }

        public OrderMock BaseOrder()
        {
            Value.Number = Fake.GetOrderNumber(Key);
            Value.Status = Fake.GetStatusOrder();
            Value.CreateDate = Fake.GetCreateDate();
            Value.Customer = CustomerMock.Get(Key);

            return this;
        }

        public OrderMock WithItem(string itemKey = null)
        {
            Value.Items.Add(OrderItemMock.Get(itemKey));

            return this;
        }
    }
}