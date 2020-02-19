using Framework.Test.Common;
using Library.Domain.CommandSide.Commands;
using Library.Tests.Utils;

namespace Library.Tests.Mocks.Commands
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