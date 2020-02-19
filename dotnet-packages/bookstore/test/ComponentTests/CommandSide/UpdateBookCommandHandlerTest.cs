﻿using Bookstore.Data;
using Bookstore.Domain.CommandSide.Commands;
using Bookstore.Domain.Entities;
using Bookstore.Infra.CrossCutting;
using Bookstore.Tests.Mocks.Commands;
using Bookstore.Tests.Mocks.Entities;
using FluentAssertions;
using Framework.Test.Common;
using Framework.Test.Data;
using Framework.Test.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using Xunit;

namespace Bookstore.Tests.ComponentTests.CommandSide
{
    [Collection(nameof(DatabaseFixture))]
    public class UpdateBookCommandHandlerTest
    {
        protected readonly IMockRepository<DbBookstore> MockRepository;

        public UpdateBookCommandHandlerTest()
        {
            MockRepository = new EfMockRepository<DbBookstore>();
        }

        [Fact]
        public void Handle_UpdateBookCommand_Default()
        {
            // arrange
            var key = MockBuilder.Key;

            var book = BookMock.Get(key);

            MockRepository.Add(book);

            MockRepository.Commit();

            var key2 = MockBuilder.Key;
            var command = UpdateBookCommandMock.Get(key2);
            command.Title = book.Title;

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var books = MockRepository.Query<Book>().ToList();
            var bookExpected = BookMock.Get(key2);
            bookExpected.Title = book.Title;
            books.Should().BeEquivalentToEntity(MockBuilder.List(bookExpected));
        }

        [Fact]
        public void Handle_UpdateBookCommand_Book_Not_Exists()
        {
            // arrange
            var key = MockBuilder.Key;

            var book = BookMock.Get(key);

            MockRepository.Add(book);

            MockRepository.Commit();

            var key2 = MockBuilder.Key;
            var command = UpdateBookCommandMock.Get(key2);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var books = MockRepository.Query<Book>().ToList();
            var bookExpected = BookMock.Get(key);
            books.Should().BeEquivalentToEntity(MockBuilder.List(bookExpected));
        }

        private bool Handle(UpdateBookCommand command)
        {
            var provider = DependencyInjectorStub.Get((s, c) =>
            {
                BootStrapper.RegisterServices(s, c);
                s.AddScoped(x => MockRepository.GetContext());
            });

            var handler = provider.GetRequiredService<IRequestHandler<UpdateBookCommand, bool>>();
            return handler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}