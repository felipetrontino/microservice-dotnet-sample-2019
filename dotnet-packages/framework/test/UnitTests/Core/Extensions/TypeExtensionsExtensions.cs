using FluentAssertions;
using Framework.Core.Extensions;
using Framework.Tests.Mocks;
using System;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Extensions
{
    public class TypeExtensionsExtensions
    {
        #region Clone

        [Fact]
        public void Clone_Obj_Valid()
        {
            // arrange
            var obj = GetObj();

            // act
            var result = Clone(obj);

            // assert
            var resultExpected = GetObj();
            result.Should().BeEquivalentTo(resultExpected);
        }

        [Fact]
        public void Clone_Obj_Null()
        {
            // arrange
            var obj = GetObjNull();

            // act
            var result = Clone(obj);

            // assert
            var resultExpected = GetObjNull();
            result.Should().BeEquivalentTo(resultExpected);
        }

        [Fact]
        public void Clone_Obj_Empty()
        {
            // arrange
            var obj = GetObjEmpty();

            // act
            var result = Clone(obj);

            // assert
            var resultExpected = GetObjEmpty();
            result.Should().BeEquivalentTo(resultExpected);
        }

        #endregion Clone

        private static T Clone<T>(T item)
        {
            return item.Clone();
        }

        #region Mocks

        private static Test GetObj()
        {
            return new Test()
            {
                Name = nameof(Test),
                Date = Fake.GetDate(),
                NullableDate = Fake.GetDate(),
                Integer = 1,
                NullableInteger = 1,
                Boolean = true,
                NullableBoolean = true
            };
        }

        private static Test GetObjNull()
        {
            return null;
        }

        private static Test GetObjEmpty()
        {
            return new Test();
        }

        public class Test
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public DateTime? NullableDate { get; set; }
            public int Integer { get; set; }
            public int? NullableInteger { get; set; }
            public bool Boolean { get; set; }
            public bool? NullableBoolean { get; set; }
        }

        #endregion Mocks
    }
}