using FluentAssertions;
using Framework.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Pagination
{
    public class PagedResponseTest
    {
        #region Constructor

        [Fact]
        public void Constructor_Source_Valid()
        {
            // arrange
            var page = 1;
            var pageSize = 15;
            var source = GetSource(pageSize);

            // act
            var result = Constructor<Data>(source, page, pageSize, source.Count);

            // assert
            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
            result.TotalCount.Should().Be(source.Count);
            result.TotalPages.Should().Be(1);
            result.HasPrevious.Should().BeFalse();
            result.HasNext.Should().BeFalse();
            result.Should().BeEquivalentTo(source);
        }

        [Fact]
        public void Constructor_Source_Empty()
        {
            // arrange
            var page = 1;
            var pageSize = 15;
            var source = GetSourceEmpty();

            // act
            var result = Constructor<Data>(source, page, pageSize, source.Count);

            // assert
            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
            result.TotalCount.Should().Be(source.Count);
            result.TotalPages.Should().Be(0);
            result.HasPrevious.Should().BeFalse();
            result.HasNext.Should().BeFalse();
            result.Should().BeEquivalentTo(source);
        }

        [Fact]
        public void Constructor_Source_Null()
        {
            // arrange
            var page = 1;
            var pageSize = 15;
            var source = GetSourceNull();

            // act
            Action action = () => Constructor<Data>(source, page, pageSize, 0);

            // assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_Source_TwoPages_FirstPage()
        {
            // arrange
            var page = 1;
            var pageSize = 15;
            var source = GetSource(pageSize + 1);

            // act
            var result = Constructor<Data>(source, page, pageSize, source.Count);

            // assert
            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
            result.TotalCount.Should().Be(source.Count);
            result.TotalPages.Should().Be(2);
            result.HasPrevious.Should().BeFalse();
            result.HasNext.Should().BeTrue();
            result.Should().BeEquivalentTo(source);
        }

        [Fact]
        public void Constructor_Source_TwoPages_SecondPage()
        {
            // arrange
            var page = 2;
            var pageSize = 15;
            var source = GetSource(pageSize + 1);

            // act
            var result = Constructor<Data>(source, page, pageSize, source.Count);

            // assert
            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
            result.TotalCount.Should().Be(source.Count);
            result.TotalPages.Should().Be(2);
            result.HasPrevious.Should().BeTrue();
            result.HasNext.Should().BeFalse();
            result.Should().BeEquivalentTo(source);
        }

        #endregion Constructor

        #region Empty

        [Fact]
        public void Empty_Valid()
        {
            // arrange

            // act
            var result = Empty<Data>();

            // assert
            var resultExpected = new PagedResponse<Data>(Enumerable.Empty<Data>(), 0, 0, 0);
            result.Should().BeEquivalentTo(resultExpected);
        }

        #endregion Empty

        private static PagedResponse<T> Constructor<T>(IEnumerable<T> rows, int page, int pageSize, int count)
        {
            return new PagedResponse<T>(rows, page, pageSize, count);
        }

        private static PagedResponse<T> Empty<T>()
        {
            return PagedResponse<T>.Empty;
        }

        #region Mocks

        private static List<Data> GetSource(int count = 1)
        {
            var ret = new List<Data>();

            for (var i = 0; i < count; i++)
            {
                ret.Add(new Data());
            }

            return ret;
        }

        private static List<Data> GetSourceEmpty()
        {
            return new List<Data>();
        }

        private static List<Data> GetSourceNull()
        {
            return null;
        }

        public class Data
        {
            public string Name { get; set; }
        }

        #endregion Mocks
    }
}