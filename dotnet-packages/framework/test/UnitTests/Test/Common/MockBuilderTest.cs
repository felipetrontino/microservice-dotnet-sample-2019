using FluentAssertions;
using Framework.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Framework.Tests.UnitTests.Test.Common
{
    public class MockBuilderTest
    {
        #region Key

        [Fact]
        public void Key_Valid()
        {
            // arrange

            // act
            var key = MockBuilder.Key;

            // assert
            var parse = Guid.TryParse(key, out var result);
            parse.Should().BeTrue();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void Key_Two_Time()
        {
            // arrange

            // act
            var key1 = MockBuilder.Key;
            var key2 = MockBuilder.Key;

            // assert
            key1.Should().NotBe(key2);
        }

        #endregion Key

        #region Date

        [Fact]
        public void Date_Valid()
        {
            // arrange

            // act
            var date = MockBuilder.Date;

            // assert
            date.Should().Be(DateTime.UtcNow.Date);
        }

        #endregion Date

        #region Keys

        [Fact]
        public void Keys_Valid()
        {
            // arrange

            // act
            var keys = MockBuilder.Keys(1);

            // assert
            keys.Should().HaveCount(1);
            keys.All(x =>
            {
                var parse = Guid.TryParse(x, out var result);
                parse.Should().BeTrue();
                result.Should().NotBeEmpty();

                return true;
            });
        }

        [Fact]
        public void Keys_Greater_Than_One()
        {
            // arrange

            // act
            var keys = MockBuilder.Keys(2);

            // assert
            keys.Should().HaveCount(2);
            keys.All(x =>
            {
                var parse = Guid.TryParse(x, out var result);
                parse.Should().BeTrue();
                result.Should().NotBeEmpty();

                return true;
            });
        }

        [Fact]
        public void Keys_Zero()
        {
            // arrange

            // act
            var keys = MockBuilder.Keys(0);

            // assert
            keys.Should().HaveCount(0);
        }

        #endregion Keys

        #region GetId

        [Fact]
        public void GetId_Key_Valid()
        {
            // arrange
            var key = MockBuilder.Key;

            // act
            var id = MockBuilder.GetId(key);

            // assert
            id.Should().Be(Guid.Parse(key));
        }

        [Fact]
        public void GetId_Key_Null()
        {
            // arrange
            var key = (string)null;

            // act
            var id = MockBuilder.GetId(key);

            // assert
            id.Should().Be(Guid.Empty);
        }

        [Fact]
        public void GetId_Key_Invalid()
        {
            // arrange
            var key = "A";

            // act
            var id = MockBuilder.GetId(key);

            // assert
            id.Should().Be(Guid.Empty);
        }

        #endregion GetId

        #region GetIncrement

        [Fact]
        public void GetIdByValue_Value_Valid()
        {
            // arrange
            MockBuilder.ResetBags();
            var key = MockBuilder.Key;

            // act
            var value = MockBuilder.GetIdByValue(key);

            // assert
            value.Should().NotBeEmpty();
        }

        [Fact]
        public void GetIdByValue_Value_Null()
        {
            // arrange
            MockBuilder.ResetBags();
            var key = (string)null;

            // act
            var value = MockBuilder.GetIdByValue(key);

            // assert
            value.Should().NotBeEmpty();
        }

        [Fact]
        public void GetIdByValue_Value_Two_Times_Equals()
        {
            // arrange
            MockBuilder.ResetBags();
            var key = MockBuilder.Key;

            // act
            var first = MockBuilder.GetIdByValue(key);
            var second = MockBuilder.GetIdByValue(key);

            // assert
            first.Should().NotBeEmpty();
            second.Should().NotBeEmpty();
            first.Should().Be(second);
        }

        [Fact]
        public void GetIdByValue_Value_Two_Times_Different()
        {
            // arrange
            MockBuilder.ResetBags();
            var key1 = MockBuilder.Key;
            var key2 = MockBuilder.Key;

            // act
            var first = MockBuilder.GetIdByValue(key1);
            var second = MockBuilder.GetIdByValue(key2);

            // assert
            first.Should().NotBeEmpty();
            second.Should().NotBeEmpty();
            first.Should().NotBe(second);
        }

        #endregion GetIncrement

        #region GetIncrement

        [Fact]
        public void GetIncrement_Key_Valid()
        {
            // arrange
            MockBuilder.ResetBags();
            var key = MockBuilder.Key;

            // act
            var value = MockBuilder.GetIncrement(key);

            // assert
            value.Should().Be(1);
        }

        [Fact]
        public void GetIncrement_Key_Null()
        {
            // arrange
            MockBuilder.ResetBags();
            var key = (string)null;

            // act
            var value = MockBuilder.GetIncrement(key);

            // assert
            value.Should().Be(0);
        }

        [Fact]
        public void GetIncrement_Key_Two_Times_Equals()
        {
            // arrange
            MockBuilder.ResetBags();
            var key = MockBuilder.Key;

            // act
            var first = MockBuilder.GetIncrement(key);
            var second = MockBuilder.GetIncrement(key);

            // assert
            first.Should().Be(1);
            second.Should().Be(1);
            first.Should().Be(second);
        }

        [Fact]
        public void GetIncrement_Key_Two_Times_Different()
        {
            // arrange
            MockBuilder.ResetBags();
            var key1 = MockBuilder.Key;
            var key2 = MockBuilder.Key;

            // act
            var first = MockBuilder.GetIncrement(key1);
            var second = MockBuilder.GetIncrement(key2);

            // assert
            first.Should().Be(1);
            second.Should().Be(2);
            first.Should().NotBe(second);
        }

        #endregion GetIncrement

        #region ResetIncrements

        [Fact]
        public void ResetBags_Valid()
        {
            // arrange
            MockBuilder.GetIncrement(MockBuilder.Key);

            // act
            MockBuilder.ResetBags();

            // assert
            var value = MockBuilder.GetIncrement(MockBuilder.Key);
            value.Should().Be(1);
        }

        #endregion ResetIncrements

        #region List

        [Fact]
        public void List_Valid()
        {
            // arrange
            var value = "A";

            // act
            var values = MockBuilder.List(value);

            // assert
            values.Should().NotBeEmpty();
            values.Should().HaveCount(1);
            values.Should().Contain(value);
        }

        [Fact]
        public void List_Multiples()
        {
            // arrange
            var value1 = "A";
            var value2 = "B";
            var value3 = "C";

            // act
            var values = MockBuilder.List(value1, value2, value3);

            // assert
            values.Should().NotBeEmpty();
            values.Should().HaveCount(3);
            values.Should().ContainInOrder(value1, value2, value3);
        }

        [Fact]
        public void List_Value_Null()
        {
            // arrange
            var value = (string)null;

            // act
            var values = MockBuilder.List(value);

            // assert
            values.Should().NotBeEmpty();
            values.Should().HaveCount(1);
            values.Should().Contain(value);
        }

        [Fact]
        public void List_Parameters_Null()
        {
            // arrange
            var parameters = (string[])null;

            // act
            var values = MockBuilder.List(parameters);

            // assert
            values.Should().BeEmpty();
        }

        #endregion List

        #region ListEmpty

        [Fact]
        public void ListEmpty_Valid()
        {
            // arrange

            // act
            var values = MockBuilder.ListEmpty<string>();

            // assert
            values.Should().BeEmpty();
            values.Should().BeEquivalentTo(new List<string>());
        }

        #endregion ListEmpty

        #region GetException

        [Fact]
        public void GetException_Valid()
        {
            // arrange
            var message = "INVALID";

            // act
            var ex = MockBuilder.GetException(message);

            // assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be(message);
            ex.Should().BeEquivalentTo(new Exception(message));
        }

        [Fact]
        public void GetException_Message_Null()
        {
            // arrange

            // act
            var ex = MockBuilder.GetException();

            // assert
            var messageExpected = "ERROR";
            ex.Should().NotBeNull();
            ex.Message.Should().Be(messageExpected);
            ex.Should().BeEquivalentTo(new Exception(messageExpected));
        }

        #endregion GetException

        #region MockBuilder´´

        [Fact]
        public void MockBuilder_Create_Valid()
        {
            // arrange
            var key = MockBuilder.Key;

            // act
            var builder = TestMock.Create(key);
            var value = builder.Build();

            // assert
            var valueExpected = GetValue(key);
            builder.Key.Should().Be(key);
            builder.Value.Should().BeEquivalentTo(valueExpected);
            value.Should().BeEquivalentTo(valueExpected);
            value.Should().Be(builder.Value);
        }

        [Fact]
        public void MockBuilder_Create_Key_Null()
        {
            // arrange
            var key = (string)null;

            // act
            var builder = TestMock.Create(key);
            var value = builder.Build();

            // assert
            var valueExpected = GetValue(key);
            builder.Key.Should().Be(key);
            builder.Value.Should().BeEquivalentTo(valueExpected);
            value.Should().BeEquivalentTo(valueExpected);
            value.Should().Be(builder.Value);
        }

        [Fact]
        public void MockBuilder_Create_Model_Without_Id()
        {
            // arrange
            var key = MockBuilder.Key;

            // act
            var builder = TestWithoutIdMock.Create(key);
            var value = builder.Build();

            // assert
            var valueExpected = GetValueWithoutId();
            builder.Key.Should().Be(key);
            builder.Value.Should().BeEquivalentTo(valueExpected);
            value.Should().BeEquivalentTo(valueExpected);
            value.Should().Be(builder.Value);
        }

        #endregion MockBuilder´´

        #region Mocks

        public static Test GetValue(string key)
        {
            return new Test() { Id = key != null ? Guid.Parse(key) : Guid.Empty };
        }

        public static TestWithoutId GetValueWithoutId()
        {
            return new TestWithoutId();
        }

        public class TestMock : MockBuilder<TestMock, Test>
        {
        }

        public class TestWithoutIdMock : MockBuilder<TestWithoutIdMock, TestWithoutId>
        {
        }

        public class Test
        {
            public Guid Id { get; set; }
        }

        public class TestWithoutId
        {
            public string Name { get; set; }
        }

        #endregion Mocks
    }
}