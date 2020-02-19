using Book.Domain.Entities;
using Framework.Test.Common;

namespace Book.Tests.Mocks.Entities
{
    public class BookCategoryBookMock : MockBuilder<BookCategoryBookMock, BookCategoryBook>
    {
        public static BookCategoryBook Get(Book.Domain.Entities.Book book, BookCategory category)
        {
            return Create().Default(book, category).Build();
        }

        public BookCategoryBookMock Default(Book.Domain.Entities.Book book, BookCategory category)
        {
            Value.Book = book;
            Value.Category = category;

            return this;
        }
    }
}