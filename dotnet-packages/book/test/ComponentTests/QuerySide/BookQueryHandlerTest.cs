using Book.Domain.Enums;
using Book.Domain.QuerySide.Queries;
using Book.Infra.CrossCutting;
using Book.Infra.Data;
using Book.Models.Dto;
using Book.Tests.Mocks;
using Book.Tests.Mocks.Entities;
using Book.Tests.Mocks.Models.Dto;
using Book.Tests.Mocks.Queries;
using FluentAssertions;
using Framework.Core.Pagination;
using Framework.Test.Common;
using Framework.Test.Data;
using Framework.Test.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Xunit;

namespace Book.Tests.ComponentTests.QuerySide
{
    [Collection(nameof(DatabaseFixture))]
    public class BookQueryHandlerTest
    {
        protected readonly IMockRepository<DbBook> MockRepository;

        public BookQueryHandlerTest()
        {
            MockRepository = new EfMockRepository<DbBook>();
        }

        #region GetBookQuery

        [Fact]
        public void Handle_GetBookQuery_Default()
        {
            // arrange
            var key = MockBuilder.Key;

            var book = BookMock.Get(key);
            MockRepository.Add(book);

            MockRepository.Commit();

            var query = GetBookQueryMock.Get(key);

            // act
            var dto = Handle(query);
            dto.Should().NotBeNull();

            var dtoExpected = BookDtoMock.Get(key);
            dto.Should().BeEquivalentTo(dtoExpected);
        }

        [Fact]
        public void Handle_GetBookQuery_Book_Not_Exists()
        {
            // arrange
            var key = MockBuilder.Key;

            var book = BookMock.Get(key);
            MockRepository.Add(book);

            MockRepository.Commit();

            var key2 = MockBuilder.Key;
            var query = GetBookQueryMock.Get(key2);

            // act
            var dto = Handle(query);
            dto.Should().BeNull();
        }

        #endregion GetBookQuery

        #region ListBookFilteringQuery

        [Fact]
        public void Handle_ListBookFilteringQuery_Default()
        {
            // arrange
            var key = MockBuilder.Key;

            var book = BookMock.Get(key);
            MockRepository.Add(book);

            MockRepository.Commit();

            var query = ListBookFilteringQueryMock.Get(key);

            // act
            var dto = Handle(query);
            dto.Should().NotBeNull();

            var dtoExpected = BookDtoMock.Get(key);
            dto.Should().BeEquivalentTo(MockBuilder.List(dtoExpected).ToPagedResponse());
        }

        [Fact]
        public void Handle_ListBookFilteringQuery_Multiple()
        {
            // arrange
            var key = MockBuilder.Key;

            var book = BookMock.Get(key);
            MockRepository.Add(book);

            var key2 = MockBuilder.Key;

            var book2 = BookMock.Get(key2);
            MockRepository.Add(book2);

            MockRepository.Commit();

            var query = ListBookFilteringQueryMock.Get(key);

            // act
            var dto = Handle(query);
            dto.Should().NotBeNull();

            var dtoExpected = BookDtoMock.Get(key);
            var dtoExpected2 = BookDtoMock.Get(key2);
            dto.Should().BeEquivalentTo(MockBuilder.List(dtoExpected, dtoExpected2).ToPagedResponse());
        }

        [Fact]
        public void Handle_ListBookFilteringQuery_Book_Not_Exists()
        {
            // arrange
            var key = MockBuilder.Key;

            var query = ListBookFilteringQueryMock.Get(key);

            // act
            var dto = Handle(query);
            dto.Should().NotBeNull();

            dto.Should().BeEquivalentTo(PagedResponse<BookDto>.Empty);
        }

        [Fact]
        public void Handle_ListBookFilteringQuery_Filter_Title()
        {
            // arrange
            var key = MockBuilder.Key;

            var book = BookMock.Get(key);
            MockRepository.Add(book);

            var key2 = MockBuilder.Key;

            var book2 = BookMock.Get(key2);
            MockRepository.Add(book2);

            MockRepository.Commit();

            var query = ListBookFilteringQueryMock.Get(key);
            query.Title = Fake.GetTitle(key2);

            // act
            var dto = Handle(query);
            dto.Should().NotBeNull();

            var dtoExpected = BookDtoMock.Get(key2);
            dto.Should().BeEquivalentTo(MockBuilder.List(dtoExpected).ToPagedResponse());
        }

        [Fact]
        public void Handle_ListBookFilteringQuery_Filter_Language()
        {
            // arrange
            var key = MockBuilder.Key;

            var book = BookMock.Get(key);
            MockRepository.Add(book);

            var key2 = MockBuilder.Key;

            var book2 = BookMock.Get(key2);
            book2.Language = Language.Portuguese;
            MockRepository.Add(book2);

            MockRepository.Commit();

            var query = ListBookFilteringQueryMock.Get(key);
            query.Language = book2.Language;

            // act
            var dto = Handle(query);
            dto.Should().NotBeNull();

            var dtoExpected = BookDtoMock.Get(key2);
            dtoExpected.Language = book2.Language;
            dto.Should().BeEquivalentTo(MockBuilder.List(dtoExpected).ToPagedResponse());
        }

        [Fact]
        public void Handle_ListBookFilteringQuery_Filter_Category()
        {
            // arrange
            var key = MockBuilder.Key;

            var book = BookMock.Create(key)
                                       .Default()
                                       .WithCategory()
                                       .Build();
            MockRepository.Add(book);

            var key2 = MockBuilder.Key;

            var book2 = BookMock.Create(key2)
                                       .Default()
                                       .WithCategory()
                                       .Build();
            MockRepository.Add(book2);

            MockRepository.Commit();

            var query = ListBookFilteringQueryMock.Get(key);
            query.Category = Fake.GetCategoryName(key2);

            // act
            var dto = Handle(query);
            dto.Should().NotBeNull();

            var dtoExpected = BookDtoMock.Get(key2);
            dto.Should().BeEquivalentTo(MockBuilder.List(dtoExpected).ToPagedResponse());
        }

        #endregion ListBookFilteringQuery

        private BookDto Handle(GetBookQuery query)
        {
            var provider = DependencyInjectorStub.Get((s, c) =>
            {
                BootStrapper.RegisterServices(s, c);
                s.AddScoped(x => MockRepository.GetContext());
            });

            var handler = provider.GetRequiredService<IRequestHandler<GetBookQuery, BookDto>>();
            return handler.Handle(query, CancellationToken.None).GetAwaiter().GetResult();
        }

        private PagedResponse<BookDto> Handle(ListBookFilteringQuery query)
        {
            var provider = DependencyInjectorStub.Get((s, c) =>
            {
                BootStrapper.RegisterServices(s, c);
                s.AddScoped(x => MockRepository.GetContext());
            });

            var handler = provider.GetRequiredService<IRequestHandler<ListBookFilteringQuery, PagedResponse<BookDto>>>();
            return handler.Handle(query, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}