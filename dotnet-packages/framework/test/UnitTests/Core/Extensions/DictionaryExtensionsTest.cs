using FluentAssertions;
using Framework.Core.Extensions;
using System.Collections.Generic;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Extensions
{
    public class DictionaryExtensionsTest
    {
        [Fact]
        public void GetValueOrDefault_Valid()
        {
            // arrange
            var dic = GetDic();
            var key = GetKey();

            // act
            var result = GetValueOrDefault(dic, key);

            // assert
            var resultExpected = GetValue();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void GetValueOrDefault_NotExists()
        {
            // arrange
            var dic = GetDic();
            var key = GetKeyWrong();

            // act
            var result = GetValueOrDefault(dic, key);

            // assert
            result.Should().Be(expected: default);
            result.Should().NotBe(GetValue());
        }

        [Fact]
        public void GetValueOrDefault_With_Default_Valid()
        {
            // arrange
            var dic = GetDic();
            var key = GetKey();
            var defaultValue = GetDefaultValue();

            // act
            var result = GetValueOrDefault(dic, key, defaultValue);

            // assert
            var resultExpected = GetValue();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void GetValueOrDefault_With_Default_NotExists()
        {
            // arrange
            var dic = GetDic();
            var key = GetKeyWrong();
            var defaultValue = GetDefaultValue();

            // act
            var result = GetValueOrDefault(dic, key, defaultValue);

            // assert
            result.Should().Be(defaultValue);
            result.Should().NotBe(GetValue());
        }

        private static TValue GetValueOrDefault<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            return DictionaryExtensions.GetValueOrDefault(dictionary, key, defaultValue);
        }

        private static Dictionary<string, string> GetDic()
        {
            return new Dictionary<string, string> { { GetKey(), GetValue() } };
        }

        private static string GetKey()
        {
            return "KEY";
        }

        private static string GetKeyWrong()
        {
            return "KEY2";
        }

        private static string GetValue()
        {
            return "VALUE";
        }

        private static string GetDefaultValue()
        {
            return "";
        }
    }
}