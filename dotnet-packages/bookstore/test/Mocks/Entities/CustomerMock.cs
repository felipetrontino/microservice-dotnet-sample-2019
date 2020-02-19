using Bookstore.Domain.Entities;
using Bookstore.Tests.Utils;
using Framework.Test.Common;

namespace Bookstore.Tests.Mocks.Entities
{
    public class CustomerMock : MockBuilder<CustomerMock, Customer>
    {
        public static Customer Get(string key)
        {
            return Create(key).Default().Build();
        }

        public static Customer GetFull(string key)
        {
            return Create(key).Full().Build();
        }

        public CustomerMock Default()
        {
            Value.Name = Fake.GetCustomerName(Key);
            Value.DocumentId = MockBuilder.GetId(Key).ToString();

            return this;
        }

        public CustomerMock Full()
        {
            Value.Name = Fake.GetCustomerName(Key);
            Value.DocumentId = MockBuilder.GetId(Key).ToString();
            Value.Address = Fake.GetAddress(Key);
            Value.City = Fake.GetCity(Key);
            Value.State = Fake.GetState(Key);
            Value.Email = Fake.GetEmail(Key);
            Value.Phone = Fake.GetPhone(Key);
            Value.UserId = MockBuilder.GetId(Key).ToString();

            return this;
        }
    }
}