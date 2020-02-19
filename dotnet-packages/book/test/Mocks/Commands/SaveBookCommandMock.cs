using Book.Domain.CommandSide.Commands;
using Framework.Test.Common;

namespace Book.Tests.Mocks.Commands
{
    public class SaveBookCommandMock : MockBuilder<SaveBookCommandMock, SaveBookCommand>
    {
        public static SaveBookCommand Get(string key)
        {
            return Create(key).Default().Build();
        }

        public SaveBookCommandMock Default()
        {
            Value.Author = Fake.GetAuthorName(Key);
            Value.Title = Fake.GetTitle(Key);
            Value.Language = Fake.GetLanguage();

            return this;
        }

        public SaveBookCommandMock WithCategory()
        {
            Value.Category = Fake.GetCategoryName(Key);

            return this;
        }
    }
}