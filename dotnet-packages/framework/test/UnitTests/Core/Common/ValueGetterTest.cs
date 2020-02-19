using FluentAssertions;
using Framework.Core.Common;
using System;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Common
{
    public class ValueGetterTest
    {
        #region Get

        [Fact]
        public void ValueGetter_Get_Valid()
        {
            // arrange
            var value = "Test";

            string Getter() => value;
            void Setter(string x) => value = x;
            var valueGetter = Constructor<string>(Getter, Setter);

            // act
            var result = valueGetter.Get();

            // assert
            var resultExpected = value;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void ValueGetter_Get_Change_After()
        {
            // arrange
            var value = "Test";
            string Getter() => value;
            void Setter(string x) => value = x;
            var valueGetter = Constructor<string>(Getter, Setter);

            // act
            var result = valueGetter.Get();

            // assert
            var resultExpected = value;
            result.Should().Be(resultExpected);

            // arrange 2
            value = "Test2";

            // act 2
            var result2 = valueGetter.Get();

            // assert 2
            var resultExpected2 = value;
            result2.Should().Be(resultExpected);
            result2.Should().NotBe(resultExpected2);
        }

        [Fact]
        public void ValueGetter_Get_Change_After_With_Recreate()
        {
            // arrange
            var value = "Test";
            string Getter() => value;
            void Setter(string x) => value = x;
            var valueGetter = Constructor<string>(Getter, Setter);

            // act
            var result = valueGetter.Get(true);

            // assert
            var resultExpected = value;
            result.Should().Be(resultExpected);

            // arrange 2
            value = "Test2";

            // act 2
            var result2 = valueGetter.Get(true);

            // assert 2
            var resultExpected2 = value;
            result2.Should().Be(resultExpected2);
            result2.Should().NotBe(resultExpected);
        }

        [Fact]
        public void ValueGetter_Get_Without_Getter()
        {
            // arrange

            static string Getter() => null;

            var valueGetter = Constructor<string>(Getter, (x) => { });

            // act
            var result = valueGetter.Get();

            // assert
            result.Should().BeNull();
        }

        #endregion Get

        #region Set

        [Fact]
        public void ValueGetter_Set_Valid()
        {
            // arrange
            var value = "Test";

            string Getter() => value;
            void Setter(string x) => value = x;
            var valueGetter = Constructor<string>(Getter, Setter);

            // act
            var setValue = "Test2";
            valueGetter.Set(setValue);

            // assert
            var resultExpected = setValue;
            value.Should().Be(resultExpected);
        }

        [Fact]
        public void ValueGetter_Set_Without_Getter()
        {
            // arrange
            var value = "Test";
            string Getter() => value;
            var valueGetter = Constructor<string>(Getter, null);

            // act
            var setValue = "Test2";
            valueGetter.Set(setValue);

            // assert
            var resultExpected = setValue;
            value.Should().NotBe(resultExpected);
        }

        #endregion Set

        private static ValueGetter<T> Constructor<T>(Func<T> getter, Action<T> onSet = null)
        {
            return new ValueGetter<T>(getter, onSet);
        }
    }
}