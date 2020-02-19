using Book.Domain.Entities;
using Framework.Test.Common;

namespace Book.Tests.Mocks.Entities
{
    public class BookMock : MockBuilder<BookMock, Book.Domain.Entities.Book>
    {
        public static Book.Domain.Entities.Book Get(string key)
        {
            return Create(key).Default().Build();
        }

        public BookMock Default()
        {
            Value.AuthorName = Fake.GetAuthorName(Key);
            Value.Title = Fake.GetTitle(Key);
            Value.Language = Fake.GetLanguage();

            return this;
        }

        public BookMock WithCategory(BookCategory category = null)
        {
            Value.Categories.Add(BookCategoryBookMock.Get(Value, category ?? BookCategoryMock.Get(Key)));

            return this;
        }
    }
}