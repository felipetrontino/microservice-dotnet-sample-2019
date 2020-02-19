using FluentAssertions;
using Framework.Core.Helpers;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Helpers
{
    public class JsonHelperTest
    {
        #region Serialize

        [Fact]
        public void Serialize_Valid()
        {
            // arrange
            var obj = GetObj();

            // act
            var result = Serialize(obj);

            // assert
            var resultExpected = GetJson();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void Serialize_Obj_Null()
        {
            // arrange
            var obj = GetObjNull();

            // act
            var result = Serialize(obj);

            // assert
            result.Should().BeNull();
        }

        #endregion Serialize

        #region JsonSerialize

        [Fact]
        public void JsonSerialize_Valid()
        {
            // arrange
            var obj = GetObj();

            // act
            var result = JsonSerialize(obj);

            // assert
            var resultExpected = GetJson();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void JsonSerialize_Obj_Null()
        {
            // arrange
            var obj = GetObjNull();

            // act
            var result = JsonSerialize(obj);

            // assert
            result.Should().BeNull();
        }

        #endregion JsonSerialize

        #region JsonDeserialize

        [Fact]
        public void JsonDeserialize_Valid()
        {
            // arrange
            var Json = GetJson();

            // act
            var result = JsonDeserialize<Test>(Json);

            // assert
            var resultExpected = GetObj();
            result.Should().BeEquivalentTo(resultExpected);
        }

        [Fact]
        public void JsonDeserializee_Json_Null()
        {
            // arrange
            var json = GetJsonNull();

            // act
            var result = JsonDeserialize<Test>(json);

            // assert
            result.Should().BeNull();
        }

        #endregion JsonDeserialize

        #region Deserialize

        [Fact]
        public void Deserialize_Valid()
        {
            // arrange
            var Json = GetJson();

            // act
            var result = Deserialize<Test>(Json);

            // assert
            var resultExpected = GetObj();
            result.Should().BeEquivalentTo(resultExpected);
        }

        [Fact]
        public void Deserialize_Json_Null()
        {
            // arrange
            var Json = GetJsonNull();

            // act
            var result = Deserialize<Test>(Json);

            // assert
            result.Should().BeNull();
        }

        #endregion Deserialize

        private static string Serialize<T>(T obj)
        {
            return JsonHelper.Serialize(obj);
        }

        private static string JsonSerialize(object obj)
        {
            return obj.JsonSerialize();
        }

        private static T Deserialize<T>(string json)
        {
            return JsonHelper.Deserialize<T>(json);
        }

        private static T JsonDeserialize<T>(string json)
        {
            return json.JsonDeserialize<T>();
        }

        #region Mock

        private static Test GetObj()
        {
            return new Test() { Name = nameof(Test) };
        }

        private static Test GetObjNull()
        {
            return null;
        }

        private static string GetJsonNull()
        {
            return null;
        }

        private static string GetJson()
        {
            return "{\"$id\":\"1\",\"Name\":\"Test\"}";
        }

        public class Test
        {
            public string Name { get; set; }
        }

        #endregion Mock
    }
}