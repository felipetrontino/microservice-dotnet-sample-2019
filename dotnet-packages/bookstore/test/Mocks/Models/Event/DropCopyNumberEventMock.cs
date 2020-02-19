using Bookstore.Domain.Models.Event;
using Bookstore.Tests.Utils;
using Framework.Test.Common;

namespace Bookstore.Tests.Mocks.Models.Event
{
    public class DropCopyNumberEventMock : MockBuilder<DropCopyNumberEventMock, DropCopyNumberEvent>
    {
        public static DropCopyNumberEvent Get(string key)
        {
            return Create(key).Default().Build();
        }

        public DropCopyNumberEventMock Default()
        {
            Value.Number = Fake.GetCopyNumber(Key);

            return this;
        }
    }
}