using Book.Domain.QuerySide.Queries;
using Framework.Test.Common;

namespace Book.Tests.Mocks.Queries
{
    public class GetBookQueryMock : MockBuilder<GetBookQueryMock, GetBookQuery>
    {
        public static GetBookQuery Get(string key)
        {
            return Create(key).Default().Build();
        }

        public GetBookQueryMock Default()
        {
            return this;
        }
    }
}