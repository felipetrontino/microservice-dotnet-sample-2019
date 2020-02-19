using Bookstore.Domain.CommandSide.Commands;
using Bookstore.Tests.Utils;
using Framework.Test.Common;

namespace Bookstore.Tests.Mocks.Commands
{
    public class UpdateBookCommandMock : MockBuilder<UpdateBookCommandMock, UpdateBookCommand>
    {
        public static UpdateBookCommand Get(string key)
        {
            return Create(key).Default().Build();
        }

        public UpdateBookCommandMock Default()
        {
            Value.Author = Fake.GetAuthorName(Key);
            Value.Title = Fake.GetTitle(Key);
            Value.Language = Fake.GetLanguage();

            return this;
        }
    }
}