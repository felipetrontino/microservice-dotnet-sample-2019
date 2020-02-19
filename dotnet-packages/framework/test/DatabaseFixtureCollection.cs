using Framework.Test.Common;
using Xunit;

namespace Framework.Tests
{
    [CollectionDefinition(nameof(DatabaseFixture))]
    public class DatabaseFixtureCollection : ICollectionFixture<DatabaseFixture>
    {
    }
}