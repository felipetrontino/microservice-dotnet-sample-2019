using FluentAssertions;
using Framework.Core.Common;
using Framework.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Extensions
{
    public class EnumExtensionsTest
    {
        #region GetNames

        [Fact]
        public void GetNames_With_Name()
        {
            // arrange
            const EnumName value = EnumName.WithName;

            // act
            var names = GetNames(value);

            // assert
            var namesExpected = new[] { "A", value.ToString() };
            names.Should().BeEquivalentTo(namesExpected);
        }

        [Fact]
        public void GetNames_Without_Name()
        {
            // arrange
            const EnumName value = EnumName.WithoutName;

            // act
            var names = GetNames(value);

            // assert
            var namesExpected = new[] { value.ToString() };
            names.Should().BeEquivalentTo(namesExpected);
        }

        [Fact]
        public void GetNames_With_Name_Null()
        {
            // arrange
            const EnumName value = EnumName.WithNameNull;

            // act
            var names = GetNames(value);

            // assert
            var namesExpected = new[] { value.ToString() };
            names.Should().BeEquivalentTo(namesExpected);
        }

        [Fact]
        public void GetNames_With_Name_Empty()
        {
            // arrange
            const EnumName value = EnumName.WithNameEmpty;

            // act
            var names = GetNames(value);

            // assert
            var namesExpected = new[] { value.ToString() };
            names.Should().BeEquivalentTo(namesExpected);
        }

        [Fact]
        public void GetNames_With_Name_WhiteSpace()
        {
            // arrange
            const EnumName value = EnumName.WithNameWhiteSpace;

            // act
            var names = GetNames(value);

            // assert
            var namesExpected = new[] { value.ToString() };
            names.Should().BeEquivalentTo(namesExpected);
        }

        #endregion GetNames

        #region GetName

        [Fact]
        public void GetName_With_Name()
        {
            // arrange
            const EnumName value = EnumName.WithName;

            // act
            var name = GetName(value);

            // assert
            var nameExpected = "A";
            name.Should().BeEquivalentTo(nameExpected);
        }

        [Fact]
        public void GetName_Without_Name()
        {
            // arrange
            const EnumName value = EnumName.WithoutName;

            // act
            var name = GetName(value);

            // assert
            var nameExpected = value.ToString();
            name.Should().BeEquivalentTo(nameExpected);
        }

        [Fact]
        public void GetName_With_Name_Null()
        {
            // arrange
            const EnumName value = EnumName.WithNameNull;

            // act
            var name = GetName(value);

            // assert
            var nameExpected = value.ToString();
            name.Should().BeEquivalentTo(nameExpected);
        }

        [Fact]
        public void GetName_With_Name_Empty()
        {
            // arrange
            const EnumName value = EnumName.WithNameEmpty;

            // act
            var name = GetName(value);

            // assert
            var nameExpected = value.ToString();
            name.Should().BeEquivalentTo(nameExpected);
        }

        [Fact]
        public void GetName_With_Name_WhiteSpace()
        {
            // arrange
            const EnumName value = EnumName.WithNameWhiteSpace;

            // act
            var name = GetName(value);

            // assert
            var nameExpected = value.ToString();
            name.Should().BeEquivalentTo(nameExpected);
        }

        #endregion GetName

        #region GetDescription

        [Fact]
        public void GetDescription_With_Name()
        {
            // arrange
            const EnumDescription value = EnumDescription.WithName;

            // act
            var name = GetDescription(value);

            // assert
            var nameExpected = "A";
            name.Should().BeEquivalentTo(nameExpected);
        }

        [Fact]
        public void GetDescription_Without_Name()
        {
            // arrange
            const EnumDescription value = EnumDescription.WithoutName;

            // act
            var name = GetDescription(value);

            // assert
            var nameExpected = value.ToString();
            name.Should().BeEquivalentTo(nameExpected);
        }

        [Fact]
        public void GetDescription_With_Name_Null()
        {
            // arrange
            const EnumDescription value = EnumDescription.WithNameNull;

            // act
            var name = GetDescription(value);

            // assert
            var nameExpected = value.ToString();
            name.Should().BeEquivalentTo(nameExpected);
        }

        [Fact]
        public void GetDescription_With_Name_Empty()
        {
            // arrange
            const EnumDescription value = EnumDescription.WithNameEmpty;

            // act
            var name = GetDescription(value);

            // assert
            var nameExpected = value.ToString();
            name.Should().BeEquivalentTo(nameExpected);
        }

        [Fact]
        public void GetDescription_With_Name_WhiteSpace()
        {
            // arrange
            const EnumDescription value = EnumDescription.WithNameWhiteSpace;

            // act
            var name = GetDescription(value);

            // assert
            var nameExpected = value.ToString();
            name.Should().BeEquivalentTo(nameExpected);
        }

        #endregion GetDescription

        private static IEnumerable<string> GetNames(Enum value)
        {
            return value.GetNames();
        }

        private static string GetName(Enum value)
        {
            return value.GetName();
        }

        private static string GetDescription(Enum value)
        {
            return value.GetDescription();
        }

        #region Mocks

        public enum EnumName
        {
            WithoutName,

            [EnumInfo("A")]
            WithName,

            [EnumInfo]
            WithNameNull,

            [EnumInfo("")]
            WithNameEmpty,

            [EnumInfo(" ")]
            WithNameWhiteSpace,

            [EnumInfo("A", "B")]
            WithTwoNames
        }

        public enum EnumDescription
        {
            WithoutName,

            [Description("A")]
            WithName,

            [Description]
            WithNameNull,

            [Description("")]
            WithNameEmpty,

            [Description(" ")]
            WithNameWhiteSpace
        }

        #endregion Mocks
    }
}