using FluentAssertions;
using Framework.Core.Extensions;
using Framework.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Extensions
{
    public class ParseExtensionsTest
    {
        #region DateTime

        #region ToDateTime

        [Fact]
        public void ToDateTime_Valid()
        {
            // arrange
            var input = Fake.GetDate().ToString(CultureInfo.InvariantCulture);

            // act
            var result = ToDateTime(input);

            // assert
            var resultExpected = Fake.GetDate();
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("1")]
        public void ToDateTime_Invalid(string input)
        {
            // arrange

            // act
            var result = ToDateTime(input);

            // assert
            var resultExpected = DateTime.MinValue;
            result.Should().Be(resultExpected);
        }

        #endregion ToDateTime

        #region ToNDateTime

        [Fact]
        public void ToNDateTime_Valid()
        {
            // arrange
            var input = Fake.GetDate().ToString(CultureInfo.InvariantCulture);

            // act
            var result = ToNDateTime(input);

            // assert
            var resultExpected = Fake.GetDate();
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("1")]
        public void ToNDateTime_Invalid(string input)
        {
            // arrange

            // act
            var result = ToNDateTime(input);

            // assert
            result.Should().BeNull();
        }

        #endregion ToNDateTime

        #endregion DateTime

        #region Decimal

        public static IEnumerable<object[]> ToDecimalUndefinedFormatData()
        {
            yield return new object[] { "-1", -1M };
            yield return new object[] { "1.5", 1.5M };
            yield return new object[] { "2,5", 2.5M };
            yield return new object[] { "3.50", 3.5M };
            yield return new object[] { "4,50", 4.5M };
            yield return new object[] { "5.999", 5.999M };
            yield return new object[] { "6,999", 6.999M };
            yield return new object[] { "7000.5", 7000.5M };
            yield return new object[] { "8000,5", 8000.5M };
            yield return new object[] { "9,000.5", 9000.5M };
            yield return new object[] { "10.000,5", 10000.5M };
        }

        #region ToDecimal

        [Fact]
        public void ToDecimal_Valid()
        {
            // arrange
            var input = Fake.GetDecimal().ToString(CultureInfo.InvariantCulture);

            // act
            var result = ToDecimal(input);

            // assert
            var resultExpected = Fake.GetDecimal();
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("01-01-0001 01:01:0001")]
        public void ToDecimal_Invalid(string input)
        {
            // arrange

            // act
            var result = ToDecimal(input);

            // assert
            var resultExpected = 0;
            result.Should().Be(resultExpected);
        }

        #endregion ToDecimal

        #region ToNDecimal

        [Fact]
        public void ToNDecimal_Valid()
        {
            // arrange
            var input = Fake.GetDecimal().ToString(CultureInfo.InvariantCulture);

            // act
            var result = ToNDecimal(input);

            // assert
            var resultExpected = Fake.GetDecimal();
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("01-01-0001 01:01:0001")]
        public void ToNDecimal_Invalid(string input)
        {
            // arrange

            // act
            var result = ToNDecimal(input);

            // assert
            result.Should().BeNull();
        }

        #endregion ToNDecimal

        #endregion Decimal

        #region Double

        public static IEnumerable<object[]> ToDoubleUndefinedFormatData()
        {
            yield return new object[] { "-1", -1D };
            yield return new object[] { "1.5", 1.5D };
            yield return new object[] { "2,5", 2.5D };
            yield return new object[] { "3.50", 3.5D };
            yield return new object[] { "4,50", 4.5D };
            yield return new object[] { "5.999", 5.999D };
            yield return new object[] { "6,999", 6.999D };
            yield return new object[] { "7000.5", 7000.5D };
            yield return new object[] { "8000,5", 8000.5D };
            yield return new object[] { "9,000.5", 9000.5D };
            yield return new object[] { "10.000,5", 10000.5D };
        }

        #region ToDouble

        [Fact]
        public void ToDouble_Valid()
        {
            // arrange
            var input = Fake.GetDouble().ToString(CultureInfo.InvariantCulture);

            // act
            var result = ToDouble(input);

            // assert
            var resultExpected = Fake.GetDouble();
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("01-01-0001 01:01:0001")]
        public void ToDouble_Invalid(string input)
        {
            // arrange

            // act
            var result = ToDouble(input);

            // assert
            var resultExpected = 0;
            result.Should().Be(resultExpected);
        }

        #endregion ToDouble

        #region ToNDouble

        [Fact]
        public void ToNDouble_Valid()
        {
            // arrange
            var input = Fake.GetDouble().ToString(CultureInfo.InvariantCulture);

            // act
            var result = ToNDouble(input);

            // assert
            var resultExpected = Fake.GetDouble();
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("01-01-0001 01:01:0001")]
        public void ToNDouble_Invalid(string input)
        {
            // arrange

            // act
            var result = ToNDouble(input);

            // assert
            result.Should().BeNull();
        }

        #endregion ToNDouble

        #endregion Double

        #region Int

        #region ToInt

        [Fact]
        public void ToInt_Valid()
        {
            // arrange
            var input = Fake.GetInteger().ToString(CultureInfo.InvariantCulture);

            // act
            var result = ToInt(input);

            // assert
            var resultExpected = Fake.GetInteger();
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("01-01-0001 01:01:0001")]
        public void ToInt_Invalid(string input)
        {
            // arrange

            // act
            var result = ToInt(input);

            // assert
            var resultExpected = 0;
            result.Should().Be(resultExpected);
        }

        #endregion ToInt

        #region ToNInt

        [Fact]
        public void ToNInt_Valid()
        {
            // arrange
            var input = Fake.GetInteger().ToString(CultureInfo.InvariantCulture);

            // act
            var result = ToNInt(input);

            // assert
            var resultExpected = Fake.GetInteger();
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("01-01-0001 01:01:0001")]
        public void ToNInt_Invalid(string input)
        {
            // arrange

            // act
            var result = ToNInt(input);

            // assert
            result.Should().BeNull();
        }

        #endregion ToNInt

        #endregion Int

        #region Boolean

        #region ToBoolean

        [Fact]
        public void ToBoolean_Valid()
        {
            // arrange
            var input = Fake.GetBoolean().ToString(CultureInfo.InvariantCulture);

            // act
            var result = ToBoolean(input);

            // assert
            var resultExpected = Fake.GetBoolean();
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("01-01-0001 01:01:0001")]
        public void ToBoolean_Invalid(string input)
        {
            // arrange

            // act
            var result = ToBoolean(input);

            // assert
            result.Should().BeFalse();
        }

        #endregion ToBoolean

        #region ToNBoolean

        [Fact]
        public void ToNBoolean_Valid()
        {
            // arrange
            var input = Fake.GetBoolean().ToString(CultureInfo.InvariantCulture);

            // act
            var result = ToNBoolean(input);

            // assert
            var resultExpected = Fake.GetBoolean();
            result.Should().Be(resultExpected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("01-01-0001 01:01:0001")]
        public void ToNBoolean_Invalid(string input)
        {
            // arrange

            // act
            var result = ToNBoolean(input);

            // assert
            result.Should().BeNull();
        }

        #endregion ToNBoolean

        #endregion Boolean

        private static DateTime ToDateTime(string input)
        {
            return input.ToDateTime();
        }

        private static DateTime? ToNDateTime(string input)
        {
            return input.ToNDateTime();
        }

        private static decimal ToDecimal(string input)
        {
            return input.ToDecimal();
        }

        private static decimal? ToNDecimal(string input)
        {
            return input.ToNDecimal();
        }

        private static double ToDouble(string input)
        {
            return input.ToDouble();
        }

        private static double? ToNDouble(string input)
        {
            return input.ToNDouble();
        }

        private static int ToInt(string input)
        {
            return input.ToInt();
        }

        private static int? ToNInt(string input)
        {
            return input.ToNInt();
        }

        private static bool ToBoolean(string input)
        {
            return input.ToBoolean();
        }

        private static bool? ToNBoolean(string input)
        {
            return input.ToNBoolean();
        }
    }
}