using FluentAssertions;
using Framework.Core.Common;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Common
{
    public class UserAccessorTest
    {
        [Fact]
        public void Constructor_Valid()
        {
            // arrange

            // act
            var result = Constructor();

            // assert
            var resultExpected = GetUserAccessor();
            result.Should().BeEquivalentTo(resultExpected);
        }

        [Fact]
        public void Constructor_With_Name()
        {
            // arrange
            const string name = "Test";

            // act
            var result = Constructor(name);

            // assert
            var resultExpected = GetUserAccessor(name);
            result.Should().BeEquivalentTo(resultExpected);
        }

        private static UserAccessor Constructor(string name = null)
        {
            return name != null ? new UserAccessor(name) : new UserAccessor();
        }

        #region Mocks

        public UserAccessor GetUserAccessor(string name = "system")
        {
            return new UserAccessor(name);
        }

        #endregion Mocks
    }
}