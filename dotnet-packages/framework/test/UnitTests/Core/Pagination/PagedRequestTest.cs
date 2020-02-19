using FluentAssertions;
using Framework.Core.Pagination;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Pagination
{
    public class PagedRequestTest
    {
        [Fact]
        public void Constructor_Valid()
        {
            // arrange

            // act
            var result = Constructor();

            // assert
            result.Page.Should().Be(PageValues.PageStart);
            result.PageSize.Should().Be(PageValues.PageSize);
        }

        private static PagedRequest Constructor()
        {
            return new PagedRequest();
        }

        #region Mocks

        public class Filter
        {
            public string Name { get; set; }
        }

        #endregion Mocks
    }
}