using FluentAssertions;
using Framework.Core.Common;
using Framework.Core.Helpers;
using Framework.Tests.Mocks;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Helpers
{
    public class EnumHelperTest
    {
        [Fact]
        public void ParseTo_Valid()
        {
            // arrange
            const string value = "Test";

            // act
            var result = ParseTo<EnumTest>(value);

            // assert
            const EnumTest resultExpected = EnumTest.Test;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ParseTo_Value_Upper()
        {
            // arrange
            var value = "Test".ToUpper();

            // act
            var result = ParseTo<EnumTest>(value);

            // assert
            const EnumTest resultExpected = EnumTest.Test;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ParseTo_Value_Lower()
        {
            // arrange
            var value = "Test".ToLower();

            // act
            var result = ParseTo<EnumTest>(value);

            // assert
            const EnumTest resultExpected = EnumTest.Test;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ParseTo_Value_Null()
        {
            // arrange
            var value = Fake.StringNull;

            // act
            var result = ParseTo<EnumTest>(value);

            // assert
            const EnumTest resultExpected = EnumTest.Unknown;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ParseTo_Value_Empty()
        {
            // arrange
            var value = Fake.StringEmpty;

            // act
            var result = ParseTo<EnumTest>(value);

            // assert
            const EnumTest resultExpected = EnumTest.Unknown;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ParseTo_Value_WhiteSpace()
        {
            // arrange
            var value = Fake.StringWhiteSpace;

            // act
            var result = ParseTo<EnumTest>(value);

            // assert
            const EnumTest resultExpected = EnumTest.Unknown;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ParseTo_Value_Alternative_With_AlternativeName()
        {
            // arrange
            const string value = "Teste";

            // act
            var result = ParseTo<EnumTestWithAlternative>(value);

            // assert
            const EnumTestWithAlternative resultExpected = EnumTestWithAlternative.Test;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ParseTo_Value_Alternative_Without_AlternativeName()
        {
            // arrange
            const string value = "Teste";

            // act
            var result = ParseTo<EnumTest>(value);

            // assert
            const EnumTest resultExpected = EnumTest.Unknown;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ParseTo_Value_Alternative_With_AlternativeName_Upper()
        {
            // arrange
            var value = "Teste".ToUpper();

            // act
            var result = ParseTo<EnumTestWithAlternative>(value);

            // assert
            const EnumTestWithAlternative resultExpected = EnumTestWithAlternative.Test;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ParseTo_Value_Alternative_With_AlternativeName_Lower()
        {
            // arrange
            var value = "Teste".ToLower();

            // act
            var result = ParseTo<EnumTestWithAlternative>(value);

            // assert
            const EnumTestWithAlternative resultExpected = EnumTestWithAlternative.Test;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ParseTo_Value_Alternative_With_AlternativeName_Incorrect()
        {
            // arrange
            const string value = "Teste1";

            // act
            var result = ParseTo<EnumTestWithAlternative>(value);

            // assert
            const EnumTestWithAlternative resultExpected = EnumTestWithAlternative.Unknown;
            result.Should().Be(resultExpected);
        }

        private static TEnum ParseTo<TEnum>(string value)
            where TEnum : struct
        {
            return EnumHelper.ParseTo<TEnum>(value);
        }

        #region Mocks

        public enum EnumTest
        {
            Unknown = 0,

            Test = 1,
        }

        public enum EnumTestWithAlternative
        {
            Unknown = 0,

            [EnumInfo("Teste")]
            Test = 1,
        }

        #endregion Mocks
    }
}