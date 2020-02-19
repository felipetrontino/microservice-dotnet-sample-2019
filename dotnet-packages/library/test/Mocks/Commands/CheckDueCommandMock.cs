using Framework.Test.Common;
using Library.Domain.CommandSide.Commands;

namespace Library.Tests.Mocks.Commands
{
    public class CheckDueCommandMock : MockBuilder<CheckDueCommandMock, CheckDueCommand>
    {
        public static CheckDueCommand Get(string key)
        {
            return Create(key).Default().Build();
        }

        public CheckDueCommandMock Default()
        {
            return this;
        }
    }
}