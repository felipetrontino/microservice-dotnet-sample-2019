using Bookstore.Domain.CommandSide.Commands;
using Bookstore.Tests.Utils;
using Framework.Test.Common;
using System.Collections.Generic;

namespace Bookstore.Tests.Mocks.Commands
{
    public class PurchaseBookCommandMock : MockBuilder<PurchaseBookCommandMock, PurchaseBookCommand>
    {
        public static PurchaseBookCommand Get(string key)
        {
            return Create(key).Default().Build();
        }

        public PurchaseBookCommandMock Default()
        {
            Value.Number = Fake.GetOrderNumber(Key);
            Value.CustomerName = Fake.GetCustomerName(Key);
            Value.CustomerId = MockBuilder.GetId(Key).ToString();
            Value.Date = Fake.GetOrderDate();
            Value.Items = new List<PurchaseBookCommand.Item>
            {
                GetItem(Key)
            };

            return this;
        }

        private PurchaseBookCommand.Item GetItem(string key)
        {
            var ret = CreateModel<PurchaseBookCommand.Item>(key);
            ret.Name = Fake.GetOrderItemName(key);
            ret.Number = Fake.GetCopyNumber(key);
            ret.Price = Fake.GetPrice();
            ret.Quantity = Fake.GetQuantity();

            return ret;
        }
    }
}