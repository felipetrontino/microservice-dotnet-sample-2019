using Book.Domain.QuerySide.Queries;
using Framework.Test.Common;

namespace Book.Tests.Mocks.Queries
{
    public class ListBookFilteringQueryMock : MockBuilder<ListBookFilteringQueryMock, ListBookFilteringQuery>
    {
        public static ListBookFilteringQuery Get(string key)
        {
            return Create(key).Default().Build();
        }

        public ListBookFilteringQueryMock Default()
        {
            return this;
        }
    }
}