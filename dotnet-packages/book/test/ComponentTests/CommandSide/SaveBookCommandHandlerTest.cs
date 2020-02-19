using Book.Domain.CommandSide.Commands;
using Book.Domain.Common;
using Book.Infra.CrossCutting;
using Book.Infra.Data;
using Book.Models.Event;
using Book.Tests.Mocks.Commands;
using Book.Tests.Mocks.Entities;
using Book.Tests.Mocks.Models.Event;
using FluentAssertions;
using Framework.Core.Bus;
using Framework.Test.Common;
using Framework.Test.Data;
using Framework.Test.Extensions;
using Framework.Test.Mock.Bus;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using Xunit;

namespace Book.Tests.ComponentTests.CommandSide
{
    [Collection(nameof(DatabaseFixture))]
    public class SaveBookCommandHandlerTest
    {
        protected readonly IMockRepository<DbBook> MockRepository;
        protected readonly BusPublisherStub Bus;

        public SaveBookCommandHandlerTest()
        {
            MockRepository = new EfMockRepository<DbBook>();
            Bus = BusPublisherStub.Create();
        }

        [Fact]
        public void Handle_BookCommand_Default()
        {
            // arrange
            var key = MockBuilder.Key;

            var command = SaveBookCommandMock.Get(key);

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var books = MockRepository.Query<Domain.Entities.Book>().ToList();
            var bookExpected = BookMock.Get(key);
            books.Should().BeEquivalentToEntity(MockBuilder.List(bookExpected));

            var events = Bus.GetAllPublished<UpdateBookEvent>(ContextNames.Exchange.Book);
            var eventExpected = UpdateBookEventMock.Get(key);
            events.Should().BeEquivalentToMessage(MockBuilder.List(eventExpected));
        }

        [Fact]
        public void Handle_BookCommand_Book_Exist()
        {
            // arrange
            var key = MockBuilder.Key;

            var book = BookMock.Get(key);

            MockRepository.Add(book);

            MockRepository.Commit();

            var key2 = MockBuilder.Key;
            var command = SaveBookCommandMock.Get(key2);
            command.Title = book.Title;

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var books = MockRepository.Query<Domain.Entities.Book>().ToList();
            var bookExpected = BookMock.Get(key2);
            bookExpected.Title = book.Title;
            books.Should().BeEquivalentToEntity(MockBuilder.List(bookExpected));

            var events = Bus.GetAllPublished<UpdateBookEvent>(ContextNames.Exchange.Book);
            var eventExpected = UpdateBookEventMock.Get(key2);
            eventExpected.Title = book.Title;
            events.Should().BeEquivalentToMessage(MockBuilder.List(eventExpected));
        }

        [Fact]
        public void Handle_BookCommand_With_Category_Existing_Category()
        {
            // arrange
            var key = MockBuilder.Key;

            var category = BookCategoryMock.Get(key);

            MockRepository.Add(category);

            MockRepository.Commit();

            var command = SaveBookCommandMock.Create(key)
                                         .Default()
                                         .WithCategory()
                                         .Build();

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var books = MockRepository.Query<Domain.Entities.Book>()
                 .Include(x => x.Categories)
                     .ThenInclude(x => x.Category)
                 .ToList();
            var bookExpected = BookMock.Create(key)
                                       .Default()
                                       .WithCategory(books[0].Categories[0].Category)
                                       .Build();
            books.Should().BeEquivalentToEntity(MockBuilder.List(bookExpected));

            var events = Bus.GetAllPublished<UpdateBookEvent>(ContextNames.Exchange.Book);
            var eventExpected = UpdateBookEventMock.Get(key);
            events.Should().BeEquivalentToMessage(MockBuilder.List(eventExpected));
        }

        [Fact]
        public void Handle_BookCommand_With_Category_Not_Existing_Category()
        {
            // arrange
            var key = MockBuilder.Key;

            var command = SaveBookCommandMock.Create(key)
                                         .Default()
                                         .WithCategory()
                                         .Build();

            // act
            var result = Handle(command);

            // assert
            result.Should().BeTrue();

            var books = MockRepository.Query<Domain.Entities.Book>()
                .Include(x => x.Categories)
                    .ThenInclude(x => x.Category)
                .ToList();
            var bookExpected = BookMock.Create(key)
                                       .Default()
                                       .WithCategory(books[0].Categories[0].Category)
                                       .Build();
            books.Should().BeEquivalentToEntity(MockBuilder.List(bookExpected));

            var events = Bus.GetAllPublished<UpdateBookEvent>(ContextNames.Exchange.Book);
            var eventExpected = UpdateBookEventMock.Get(key);
            events.Should().BeEquivalentToMessage(MockBuilder.List(eventExpected));
        }

        private bool Handle(SaveBookCommand command)
        {
            var provider = DependencyInjectorStub.Get((s, c) =>
            {
                BootStrapper.RegisterServices(s, c);
                s.AddScoped(x => MockRepository.GetContext());
                s.AddScoped<IBusPublisher>(x => Bus);
            });

            var handler = provider.GetRequiredService<IRequestHandler<SaveBookCommand, bool>>();
            return handler.Handle(command, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}