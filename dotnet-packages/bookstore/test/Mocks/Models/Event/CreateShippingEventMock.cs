using Bookstore.Domain.Models.Event;
using Bookstore.Tests.Utils;
using Framework.Test.Common;
using System.Collections.Generic;

namespace Bookstore.Tests.Mocks.Models.Event
{
    public class CreateShippingEventMock : MockBuilder<CreateShippingEventMock, CreateShippingEvent>
    {
        public static CreateShippingEvent Get(string key)
        {
            return Create(key).Default().Build();
        }

        public CreateShippingEventMock Default()
        {
            Value.Number = Fake.GetOrderNumber(Key);
            Value.Status = Fake.GetStatusOrder();
            Value.CreateDate = Fake.GetCreateDate();
            Value.Customer = GetCustomerDetail();

            Value.Items = new List<CreateShippingEvent.OrderItemDetail>
            {
                GetOrderItemDetail()
            };

            return this;
        }

        private CreateShippingEvent.CustomerDetail GetCustomerDetail()
        {
            var ret = CreateModel<CreateShippingEvent.CustomerDetail>(Key);
            ret.Name = Fake.GetCustomerName(Key);

            return ret;
        }

        private CreateShippingEvent.OrderItemDetail GetOrderItemDetail()
        {
            var ret = CreateModel<CreateShippingEvent.OrderItemDetail>(Key);
            ret.Name = Fake.GetOrderItemName(Key);
            ret.Price = Fake.GetPrice();
            ret.Quantity = Fake.GetQuantity();
            ret.Total = Fake.GetTotal();

            return ret;
        }
    }
}