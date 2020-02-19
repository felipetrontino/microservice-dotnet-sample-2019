using Bookstore.Domain.Entities;
using Bookstore.Tests.Utils;
using Framework.Test.Common;

namespace Bookstore.Tests.Mocks.Entities
{
    public class OrderItemMock : MockBuilder<OrderItemMock, OrderItem>
    {
        public static OrderItem Get(string key)
        {
            return Create(key).Default().Build();
        }

        public OrderItemMock Default()
        {
            Value.Name = Fake.GetOrderItemName(Key);
            Value.Price = Fake.GetPrice();
            Value.Quantity = Fake.GetQuantity();
            Value.Total = Fake.GetTotal();

            return this;
        }
    }
}