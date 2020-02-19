using FluentAssertions;
using Framework.Core.Pagination;
using MockQueryable.NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Pagination
{
    public class PaginationHelperTest
    {
        #region ToPagedResponse

        [Fact]
        public void ToPagedResponse_Valid()
        {
            // arrange
            var source = GetSource();

            // act
            var result = ToPagedResponse(source);

            // assert
            result.Page.Should().Be(PageValues.PageStart);
            result.PageSize.Should().Be(PageValues.PageSize);
            result.TotalCount.Should().Be(source.Count);
            result.TotalPages.Should().Be(1);
            result.HasPrevious.Should().BeFalse();
            result.HasNext.Should().BeFalse();
            result.Should().BeEquivalentTo(source);
        }

        [Fact]
        public void ToPagedResponse_Source_Null()
        {
            // arrange
            var source = GetSourceNull();

            // act
            Action action = () => ToPagedResponse(source);

            // assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToPagedResponse_Source_Empty()
        {
            // arrange
            var source = GetSourceEmpty();

            // act
            var result = ToPagedResponse(source);

            // assert
            result.Page.Should().Be(PageValues.PageStart);
            result.PageSize.Should().Be(PageValues.PageSize);
            result.TotalCount.Should().Be(0);
            result.TotalPages.Should().Be(0);
            result.HasPrevious.Should().BeFalse();
            result.HasNext.Should().BeFalse();
            result.Should().BeEquivalentTo(source);
        }

        #endregion ToPagedResponse

        #region ToPagedResponseAsync

        [Fact]
        public void ToPagedResponseAsync_Valid()
        {
            // arrange
            var query = GetSourceQuery();
            var pagination = GetPagination();

            // act
            var result = ToPagedResponseAsync<TestModel, TestResult>(query, pagination, MapToResult);

            // assert
            result.Page.Should().Be(pagination.Page);
            result.PageSize.Should().Be(pagination.PageSize);
            result.TotalCount.Should().Be(query.Count());
            result.TotalPages.Should().Be(1);
            result.HasPrevious.Should().BeFalse();
            result.HasNext.Should().BeFalse();

            var resultExpected = GetResult();
            result.Should().BeEquivalentTo(resultExpected);
        }

        [Fact]
        public void ToPagedResponseAsync_Query_Null()
        {
            // arrange
            var query = GetSourceQueryNull();
            var pagination = GetPagination();

            // act
            Action action = () => ToPagedResponseAsync<TestModel, TestResult>(query, pagination, MapToResult);

            // assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToPagedResponseAsync_Query_Empty()
        {
            // arrange
            var query = GetSourceQueryEmpty();
            var pagination = GetPagination();

            // act
            var result = ToPagedResponseAsync<TestModel, TestResult>(query, pagination, MapToResult);

            // assert
            result.Page.Should().Be(pagination.Page);
            result.PageSize.Should().Be(pagination.PageSize);
            result.TotalCount.Should().Be(query.Count());
            result.TotalPages.Should().Be(0);
            result.HasPrevious.Should().BeFalse();
            result.HasNext.Should().BeFalse();

            result.Should().BeEmpty();
        }

        [Fact]
        public void ToPagedResponseAsync_Data_Null()
        {
            // arrange
            var query = GetSourceQueryWithNull();
            var pagination = GetPagination();

            // act
            Action action = () => ToPagedResponseAsync<TestModel, TestResult>(query, pagination, MapToResult);

            // assert
            action.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ToPagedResponseAsync_Map_Null()
        {
            // arrange
            var query = GetSourceQuery();
            var pagination = GetPagination();

            // act
            Action action = () => ToPagedResponseAsync<TestModel, TestResult>(query, pagination, null);

            // assert
            action.Should().Throw<NullReferenceException>();
        }

        #endregion ToPagedResponseAsync

        private static PagedResponse<T> ToPagedResponse<T>(IEnumerable<T> source, int page = PageValues.PageStart, int pageSize = PageValues.PageSize)

        {
            return source.ToPagedResponse<T>(page, pageSize);
        }

        private static PagedResponse<TProxy> ToPagedResponseAsync<TEntity, TProxy>(IQueryable<TEntity> query, IPagedRequest pagination, Func<TEntity, TProxy> map)

        {
            return query.ToPagedResponseAsync<TEntity, TProxy>(pagination, map).GetAwaiter().GetResult();
        }

        #region Mocks

        private static List<TestModel> GetSource(int count = 1)
        {
            var ret = new List<TestModel>();

            for (var i = 0; i < count; i++)
            {
                ret.Add(new TestModel() { Name = $"Name {i}" });
            }

            return ret;
        }

        private static List<TestModel> GetSourceEmpty()
        {
            return new List<TestModel>();
        }

        private static List<TestModel> GetSourceNull()
        {
            return null;
        }

        private static PagedRequest GetPagination()
        {
            return new PagedRequest();
        }

        private static TestResult MapToResult(TestModel model)
        {
            return new TestResult()
            {
                Name = model.Name
            };
        }

        private static List<TestResult> GetResult(int count = 1)
        {
            var ret = new List<TestResult>();

            for (var i = 0; i < count; i++)
            {
                ret.Add(new TestResult() { Name = $"Name {i}" });
            }

            return ret;
        }

        private static IQueryable<TestModel> GetSourceQuery(int count = 1)
        {
            return GetSource(count).AsQueryable().BuildMock();
        }

        private static IQueryable<TestModel> GetSourceQueryEmpty()
        {
            return GetSourceEmpty().AsQueryable().BuildMock();
        }

        private static IQueryable<TestModel> GetSourceQueryNull()
        {
            return null;
        }

        private static IQueryable<TestModel> GetSourceQueryWithNull()
        {
            var ret = new List<TestModel>() { null };
            return ret.AsQueryable().BuildMock();
        }

        public class TestModel
        {
            public string Name { get; set; }
        }

        public class TestResult
        {
            public string Name { get; set; }
        }

        #endregion Mocks
    }
}