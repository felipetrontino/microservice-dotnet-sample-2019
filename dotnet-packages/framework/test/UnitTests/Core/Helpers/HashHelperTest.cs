using FluentAssertions;
using Framework.Core.Helpers;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Helpers
{
    public class HashHelperTest
    {
        [Fact]
        public void Create_Valid()
        {
            //arrange
            var obj = GetObj();

            //act
            var result = Create(obj);

            //assert
            var expected = GetHash();
            result.Should().Be(expected);
        }

        [Fact]
        public void Create_Invalid()
        {
            //arrange
            var obj = GetObj("TEST2");

            //act
            var result = Create(obj);

            //assert
            var expected = GetHash();
            result.Should().NotBe(expected);
        }

        private static string Create(object obj)
        {
            return HashHelper.Create(obj);
        }

        #region Mocks

        private static object GetObj(string value = "TEST")
        {
            return new { value };
        }

        private static string GetHash(string value = "TEST")
        {
            var obj = GetObj(value);
            var responseJson = JsonConvert.SerializeObject(obj);

            using var algorithm = MD5.Create();
            var hashBytes = algorithm.ComputeHash(Encoding.Default.GetBytes(responseJson));

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        #endregion Mocks
    }
}