using FluentAssertions;
using Framework.Core.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Extensions
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData(" ", false)]
        [InlineData("A", false)]
        public void IsEmpty_Test(string input, bool resultExpected)
        {
            // arrange

            // act
            var result = IsEmpty(input);

            // assert
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData(" ", true)]
        [InlineData("A", false)]
        public void IsEmptyTrim_Test(string input, bool resultExpected)
        {
            // arrange

            // act
            var result = IsEmptyTrim(input);

            // assert
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("0", "0")]
        [InlineData("1", "1")]
        [InlineData("10", "10")]
        [InlineData("01", "1")]
        [InlineData("010", "10")]
        public void DiscardLeadingZeroes_Test(string input, string resultExpected)
        {
            // arrange

            // act
            var result = DiscardLeadingZeroes(input);

            // assert
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null, 0, null)]
        [InlineData("", 0, "")]
        [InlineData(" ", 0, "")]
        [InlineData("ABC", 3, "ABC")]
        [InlineData("ABC", 2, "AB")]
        [InlineData("ABC", 1, "A")]
        [InlineData("ABC", 0, "")]
        public void Truncate_Test(string input, int maxLength, string resultExpected)
        {
            // arrange

            // act
            var result = Truncate(input, maxLength);

            // assert
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("a", "A")]
        [InlineData("A", "A")]
        [InlineData("teste", "Teste")]
        [InlineData("Teste", "Teste")]
        [InlineData("TESTE", "Teste")]
        [InlineData("result expected", "ResultExpected")]
        [InlineData("Result Expected", "ResultExpected")]
        [InlineData("RESULT EXPECTED", "ResultExpected")]
        public void ToPascalCase_Test(string input, string resultExpected)
        {
            // arrange

            // act
            var result = ToPascalCase(input);

            // assert
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("a", "a")]
        [InlineData("A", "a")]
        [InlineData("teste", "teste")]
        [InlineData("Teste", "teste")]
        [InlineData("TESTE", "teste")]
        [InlineData("result expected", "resultExpected")]
        [InlineData("Result Expected", "resultExpected")]
        [InlineData("RESULT EXPECTED", "resultExpected")]
        public void ToCamelCase_Test(string input, string resultExpected)
        {
            // arrange

            // act
            var result = ToCamelCase(input);

            // assert
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("A", "A")]
        public void ToBytes_Test(string input, string expected)
        {
            // arrange

            // act
            var result = ToBytes(input);

            // assert
            var resultExpected = expected != null ? Encoding.UTF8.GetBytes(expected) : (byte[])null;
            result.Should().BeEquivalentTo(resultExpected);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("A", "A")]
        public void ToStream_Test(string input, string resultExpected)
        {
            // arrange

            // act
            var result = ToStream(input);

            // assert
            var expected = GetString(result);
            expected.Should().Be(resultExpected);
        }

        private static bool IsEmpty(string value)
        {
            return value.IsEmpty();
        }

        private static bool IsEmptyTrim(string value)
        {
            return value.IsEmptyTrim();
        }

        private static string DiscardLeadingZeroes(string value)
        {
            return value.DiscardLeadingZeroes();
        }

        private static string Truncate(string value, int maxLength)
        {
            return value.Truncate(maxLength);
        }

        private static string ToPascalCase(string value)
        {
            return value.ToPascalCase();
        }

        private static string ToCamelCase(string value)
        {
            return value.ToCamelCase();
        }

        private static IEnumerable<byte> ToBytes(string value)
        {
            return value.ToBytes();
        }

        private static Stream ToStream(string value)
        {
            return value.ToStream();
        }

        #region Mocks

        private static string GetString(Stream ms)
        {
            if (ms == null) return null;

            using var reader = new StreamReader(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return reader.ReadToEnd();
        }

        #endregion Mocks
    }
}