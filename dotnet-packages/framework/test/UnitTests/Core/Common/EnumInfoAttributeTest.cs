using FluentAssertions;
using Framework.Core.Common;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Common
{
    public class EnumInfoAttributeTest
    {
        [Fact]
        public void Constructor_Valid()
        {
            // arrange
            var alternativeNames = new[] { "A", "B" };
            // act
            var result = Constructor(alternativeNames);

            // assert
            var resultExpected = GetEnumInfo(alternativeNames);
            result.Should().BeEquivalentTo(resultExpected);
        }

        private static EnumInfoAttribute Constructor(params string[] alternativeNames)
        {
            return new EnumInfoAttribute(alternativeNames);
        }

        #region Mocks

        public EnumInfoAttribute GetEnumInfo(params string[] alternativeNames)
        {
            return new EnumInfoAttribute(alternativeNames);
        }

        #endregion Mocks
    }
}