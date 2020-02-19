using FluentAssertions;
using Framework.Core.Config;
using Framework.Core.Extensions;
using Framework.Tests.Mocks;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Extensions
{
    public class ConfigurationExtensionsTest
    {
        #region IsFeatureEnabled

        [Fact]
        public void IsFeatureEnabled_Valid()
        {
            // arrange
            var name = GetConfigName();
            var config = GetConfig(GetValue(name, true.ToString(), Configuration.SectionNames.Feature));

            // act
            var result = IsFeatureEnabled(config, name);

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsFeatureEnabled_False()
        {
            // arrange
            var name = GetConfigName();
            var config = GetConfig(GetValue(name, false.ToString(), Configuration.SectionNames.Feature));

            // act
            var result = IsFeatureEnabled(config, name);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsFeatureEnabled_Not_Exists()
        {
            // arrange
            var name = GetConfigNameNotExists();
            var config = GetConfig(GetValue(GetConfigName(), true.ToString(), Configuration.SectionNames.Feature));

            // act
            var result = IsFeatureEnabled(config, name);

            // assert
            result.Should().BeFalse();
        }

        #endregion IsFeatureEnabled

        #region ToDictionaryConfig

        [Fact]
        public void ToDictionaryConfig_Valid()
        {
            // arrange
            var obj = GetObj();

            // act
            var dic = ToDictionaryConfig(obj);

            // assert
            var dicExpected = GetDic();
            dic.Should().BeEquivalentTo(dicExpected);
        }

        [Fact]
        public void ToDictionaryConfig_Obj_Null()
        {
            // arrange
            var obj = GetObjNull();

            // act
            var dic = ToDictionaryConfig(obj);

            // assert
            var dicExpected = GetDicEmpty();
            dic.Should().BeEquivalentTo(dicExpected);
        }

        [Fact]
        public void ToDictionaryConfig_Obj_With_Null()
        {
            // arrange
            var obj = GetObjWithNull();

            // act
            var dic = ToDictionaryConfig(obj);

            // assert
            var dicExpected = GetDicWithNull();
            dic.Should().BeEquivalentTo(dicExpected);
        }

        [Fact]
        public void ToDictionaryConfig_Obj_Dictionary()
        {
            // arrange
            var obj = GetDic();

            // act
            var dic = ToDictionaryConfig(obj);

            // assert
            var dicExpected = GetDic();
            dic.Should().BeEquivalentTo(dicExpected);
        }

        [Fact]
        public void ToDictionaryConfig_Obj_Dictionary_With_Null()
        {
            // arrange
            var obj = GetDicWithNull();

            // act
            var dic = ToDictionaryConfig(obj);

            // assert
            var dicExpected = GetDicWithNull();
            dic.Should().BeEquivalentTo(dicExpected);
        }

        [Fact]
        public void ToDictionaryConfig_With_One_SectionName()
        {
            // arrange
            var obj = GetObj();
            var sectionNames = new string[] { "A" };

            // act
            var dic = ToDictionaryConfig(obj, sectionNames);

            // assert
            var dicExpected = GetDic("A");
            dic.Should().BeEquivalentTo(dicExpected);
        }

        [Fact]
        public void ToDictionaryConfig_With_Two_SectionName()
        {
            // arrange
            var obj = GetObj();
            var sectionNames = new string[] { "A", "B" };

            // act
            var dic = ToDictionaryConfig(obj, sectionNames);

            // assert
            var dicExpected = GetDic("A", "B");
            dic.Should().BeEquivalentTo(dicExpected);
        }

        #endregion ToDictionaryConfig

        private static bool IsFeatureEnabled(IConfiguration value, string name)
        {
            return value.IsFeatureEnabled(name);
        }

        private static Dictionary<string, string> ToDictionaryConfig<T>(T obj, params string[] sectionNames)
        {
            return obj.ToDictionaryConfig(sectionNames);
        }

        #region Mocks

        private static string GetUrl()
        {
            return "www.api.com.br";
        }

        private static string GetConfigName()
        {
            return nameof(Test.Name).ToLower();
        }

        private static string GetConfigNameNotExists()
        {
            return "X".ToLower();
        }

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

        private static Test GetObjEmpty()
        {
            return new Test();
        }

        private static Test GetObjNull()
        {
            return null;
        }

        private static TestNull GetObjWithNull()
        {
            return new TestNull();
        }

        private static Dictionary<string, string> GetDic(params string[] sectionNames)
        {
            var xSection = sectionNames != null && sectionNames.Any() ? string.Join(":", sectionNames) + ":" : string.Empty;

            var dic = new Dictionary<string, string>
            {
                {(xSection + nameof(Test.Name)).ToLower(), nameof(Test)},
                {(xSection + nameof(Test.Date)).ToLower(), Fake.GetDate().ToString()},
                {(xSection + nameof(Test.NullableDate)).ToLower(), Fake.GetDate().ToString()},
                {(xSection + nameof(Test.Integer)).ToLower(), 1.ToString()},
                {(xSection + nameof(Test.NullableInteger)).ToLower(), 1.ToString()},
                {(xSection + nameof(Test.Boolean)).ToLower(), true.ToString()},
                {(xSection + nameof(Test.NullableBoolean)).ToLower(), true.ToString()}
            };

            return dic;
        }

        private static Dictionary<string, string> GetValue(string key, string value, params string[] sectionNames)
        {
            var xSection = sectionNames != null && sectionNames.Any() ? string.Join(":", sectionNames) + ":" : string.Empty;

            var dic = new Dictionary<string, string>
            {
                {(xSection + key.ToLower()), value }
            };

            return dic;
        }

        private static Dictionary<string, string> GetDicEmpty()
        {
            return new Dictionary<string, string>();
        }

        private static Dictionary<string, string> GetDicWithNull()
        {
            return new Dictionary<string, string>()
            {
                {nameof(Test.Name).ToLower(), null}
            };
        }

        private static IConfiguration GetConfig(Dictionary<string, string> dic = null)
        {
            dic ??= GetDic();

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(dic)
                .Build();

            return config;
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

        public class TestNull
        {
            public string Name { get; set; }
        }

        #endregion Mocks
    }
}