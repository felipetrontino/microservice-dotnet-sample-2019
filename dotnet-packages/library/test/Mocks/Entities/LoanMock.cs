using Framework.Test.Common;
using Library.Domain.Entities;
using Library.Tests.Utils;

namespace Library.Tests.Mocks.Entities
{
    public class LoanMock : MockBuilder<LoanMock, Loan>
    {
        public static Loan Get(string key)
        {
            return Create(key).Default().Build();
        }

        public LoanMock Default()
        {
            Value.DueDate = Fake.GetDueDate();
            Value.ReturnDate = null;
            Value.Copy = CopyMock.Get(Key);

            return this;
        }
    }
}