using FluentAssertions;
using Framework.Core.Helpers;
using Framework.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Helpers
{
    public class CsvHelperTest
    {
        #region Serialize

        [Fact]
        public void Serialize_Valid()
        {
            // arrange
            var list = GetListWithObj();

            // act
            var result = Serialize(list);

            // assert
            var resultExpected = GetFileString();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void Serialize_List_Null()
        {
            // arrange
            var list = GetListNull();

            // act
            var result = Serialize(list);

            // assert
            var resultExpected = Fake.StringEmpty;
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void Serialize_List_Empty()
        {
            // arrange
            var list = GetListEmpty();

            // act
            var result = Serialize(list);

            // assert
            var resultExpected = GetFileStringEmpty();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void Serialize_List_With_Obj_Empty()
        {
            // arrange
            var list = GetListWithObjEmpty();

            // act
            var result = Serialize(list);

            // assert
            var resultExpected = GetFileStringWithObjEmpty();
            result.Should().Be(resultExpected);
        }

        #endregion Serialize

        private static string Serialize<T>(IEnumerable<T> list)
        {
            return CsvHelper.Serialize(list);
        }

        public static List<Test> GetListNull()
        {
            return null;
        }

        public static List<Test> GetListEmpty()
        {
            return new List<Test>();
        }

        public static List<Test> GetListWithObjEmpty()
        {
            return new[] { new Test() }.ToList();
        }

        public static List<Test> GetListWithObj()
        {
            return new[] { new Test()
            {
                Name = nameof(Test),
                Text =nameof(Test),
                Date = Fake.GetDate(),
                NullableDate = Fake.GetDate(),
                Integer = 1,
                NullableInteger = 1,
                Boolean = true,
                NullableBoolean = true
            }}.ToList();
        }

        public static string GetFileString()
        {
            var date = Fake.GetDate();
            return $"Name;DisplayName;Date;NullableDate;Integer;NullableInteger;Boolean;NullableBoolean\r\nTest;Test;{date};{date};1;1;True;True\r\n";
        }

        public static string GetFileStringEmpty()
        {
            return "Name;DisplayName;Date;NullableDate;Integer;NullableInteger;Boolean;NullableBoolean\r\n";
        }

        public static string GetFileStringWithObjEmpty()
        {
            return "Name;DisplayName;Date;NullableDate;Integer;NullableInteger;Boolean;NullableBoolean\r\n;;01/01/0001 00:00:00;;0;;False;\r\n";
        }
    }

    public class Test
    {
        public string Name { get; set; }

        [DisplayName("DisplayName")]
        public string Text { get; set; }

        public DateTime Date { get; set; }
        public DateTime? NullableDate { get; set; }
        public int Integer { get; set; }
        public int? NullableInteger { get; set; }
        public bool Boolean { get; set; }
        public bool? NullableBoolean { get; set; }
    }
}