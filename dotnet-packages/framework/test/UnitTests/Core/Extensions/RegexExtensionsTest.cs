using FluentAssertions;
using Framework.Core.Extensions;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Extensions
{
    public class RegexExtensionsTest
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("  ", "  ")]
        [InlineData("!@#$%¨&*():;<>?/-_=+'}{[]ªº°§", "!@#$%¨&*():;<>?/-_=+'}{[]ªº°§")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVXZYW", "ABCDEFGHIJKLMNOPQRSTUVXZYW")]
        [InlineData("abcdefghijklmnopqrstuvxzyw", "abcdefghijklmnopqrstuvxzyw")]
        [InlineData("1234567890", "1234567890")]
        [InlineData("ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÃÕÇ", "AEIOUAEIOUAEIOUAOC")]
        [InlineData("áéíóúàèìòùâêîôûãõç", "aeiouaeiouaeiouaoc")]
        public void RemoveDiacritics_Test(string input, string resultExpected)
        {
            // arrange

            // act
            var result = RemoveDiacritics(input);

            // assert
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("  ", "  ")]
        [InlineData("!@#$%¨&*():;<>?/-_=+'}{[]ªº°§", "!@#$%¨&*():;<>?/-_=+'}{[]ªº°§")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVXZYW", "ABCDEFGHIJKLMNOPQRSTUVXZYW")]
        [InlineData("abcdefghijklmnopqrstuvxzyw", "abcdefghijklmnopqrstuvxzyw")]
        [InlineData("1234567890", "")]
        [InlineData("ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÃÕÇ", "ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÃÕÇ")]
        [InlineData("áéíóúàèìòùâêîôûãõç", "áéíóúàèìòùâêîôûãõç")]
        public void RemoveNumeric_Test(string input, string resultExpected)
        {
            // arrange

            // act
            var result = RemoveNumeric(input);

            // assert
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData("  ", "")]
        [InlineData("!@#$%¨&*():;<>?/-_=+'}{[]ªº°§", "")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVXZYW", "")]
        [InlineData("abcdefghijklmnopqrstuvxzyw", "")]
        [InlineData("1234567890", "1234567890")]
        [InlineData("ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÃÕÇ", "")]
        [InlineData("áéíóúàèìòùâêîôûãõç", "")]
        public void RemoveAlphabetic_Test(string input, string resultExpected)
        {
            // arrange

            // act
            var result = RemoveAlphabetic(input);

            // assert
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", "")]
        [InlineData("  ", "")]
        [InlineData("!@#$%¨&*():;<>?/-_=+'}{[]ªº°§", "!@#$%¨&*():;<>?/-_=+'}{[]ªº°§")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVXZYW", "ABCDEFGHIJKLMNOPQRSTUVXZYW")]
        [InlineData("abcdefghijklmnopqrstuvxzyw", "abcdefghijklmnopqrstuvxzyw")]
        [InlineData("1234567890", "1234567890")]
        [InlineData("ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÃÕÇ", "ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÃÕÇ")]
        [InlineData("áéíóúàèìòùâêîôûãõç", "áéíóúàèìòùâêîôûãõç")]
        public void RemoveSpaces_Test(string input, string resultExpected)
        {
            // arrange

            // act
            var result = RemoveSpaces(input);

            // assert
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("  ", " ")]
        [InlineData("   ", " ")]
        [InlineData("!@#$%¨&*():;<>?/-_=+'}{[]ªº°§", "!@#$%¨&*():;<>?/-_=+'}{[]ªº°§")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVXZYW", "ABCDEFGHIJKLMNOPQRSTUVXZYW")]
        [InlineData("abcdefghijklmnopqrstuvxzyw", "abcdefghijklmnopqrstuvxzyw")]
        [InlineData("1234567890", "1234567890")]
        [InlineData("ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÃÕÇ", "ÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÃÕÇ")]
        [InlineData("áéíóúàèìòùâêîôûãõç", "áéíóúàèìòùâêîôûãõç")]
        public void RemoveDoubleSpaces_Test(string input, string resultExpected)
        {
            // arrange

            // act
            var result = RemoveDoubleSpaces(input);

            // assert
            result.Should().Be(resultExpected);
        }

        private static string RemoveDiacritics(string value)
        {
            return value.RemoveDiacritics();
        }

        private static string RemoveNumeric(string value)
        {
            return value.RemoveNumeric();
        }

        private static string RemoveAlphabetic(string value)
        {
            return value.RemoveAlphabetic();
        }

        private static string RemoveSpaces(string value)
        {
            return value.RemoveSpaces();
        }

        private static string RemoveDoubleSpaces(string value)
        {
            return value.RemoveDoubleSpaces();
        }
    }
}