using Book.Models.Dto;
using Framework.Test.Common;

namespace Book.Tests.Mocks.Models.Dto
{
    public class BookDtoMock : MockBuilder<BookDtoMock, BookDto>
    {
        public static BookDto Get(string key)
        {
            return Create(key).Default().Build();
        }

        public BookDtoMock Default()
        {
            Value.Author = Fake.GetAuthorName(Key);
            Value.Title = Fake.GetTitle(Key);
            Value.Language = Fake.GetLanguage();

            return this;
        }
    }
}