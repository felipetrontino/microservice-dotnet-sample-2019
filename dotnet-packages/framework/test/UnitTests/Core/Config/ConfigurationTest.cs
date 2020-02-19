using FluentAssertions;
using Framework.Core.Common;
using Framework.Core.Config;
using Framework.Core.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;

namespace Framework.Tests.UnitTests.Config
{
    public class ConfigurationTest
    {
        public ConfigurationTest()
        {
            Configuration.Stage.Set(EnvironmentType.Development);
        }

        #region EV

        [Fact]
        public void Stage_Valid()
        {
            // arrange
            var environmentValue = EnvironmentType.Production;
            Environment.SetEnvironmentVariable(Configuration.EnvironmentVariables.Environment, environmentValue.ToString());

            // act
            var result = Stage();

            // assert
            result.Should().Be(environmentValue);
        }

        [Fact]
        public void AppName_Valid()
        {
            // arrange
            const string appName = "test";
            Environment.SetEnvironmentVariable(Configuration.EnvironmentVariables.Application, appName);

            // act
            var result = AppName();

            // assert
            const string resultExpected = "Test";
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void Debugging_Valid()
        {
            // arrange
            const bool debugValue = true;
            Environment.SetEnvironmentVariable(Configuration.EnvironmentVariables.Debug, debugValue.ToString());

            // act
            var result = Debugging();

            // assert
            result.Should().Be(debugValue);
        }

        [Fact]
        public void DebuggingSql_Valid()
        {
            // arrange
            const bool debugValue = true;
            Environment.SetEnvironmentVariable(Configuration.EnvironmentVariables.DebugSql, debugValue.ToString());

            // act
            var result = DebuggingSql();

            // assert
            result.Should().Be(debugValue);
        }

        #endregion EV

        #region GetConfiguration

        [Fact]
        public void GetConfiguration_Valid()
        {
            // arrange

            // act
            var result = GetConfiguration();

            // assert
            var configExpected = GetConfigBuilder().Build();
            result.Should().BeEquivalentTo(configExpected);
        }

        #endregion GetConfiguration

        #region PrepareBuilder

        [Fact]
        public void PrepareBuilder_Valid()
        {
            // arrange
            var config = new ConfigurationBuilder();

            // act
            var result = PrepareBuilder(config);

            // assert
            var configExpected = GetConfigBuilder();
            result.Should().BeEquivalentTo(configExpected);
        }

        #endregion PrepareBuilder

        private static EnvironmentType Stage()
        {
            return Configuration.Stage.Get(true);
        }

        private static string AppName()
        {
            return Configuration.AppName.Get(true);
        }

        private static bool Debugging()
        {
            return Configuration.Debugging.Get(true);
        }

        private static bool DebuggingSql()
        {
            return Configuration.DebuggingSql.Get(true);
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return Configuration.GetConfiguration();
        }

        private static IConfigurationBuilder PrepareBuilder(IConfigurationBuilder config)
        {
            return config.PrepareBuilder();
        }

        #region Mocks

        private static IConfigurationBuilder GetConfigBuilder()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());

            builder.AddJsonFile("appSettings.json", true, true)
                   .AddJsonFile($"appSettings.{Configuration.Stage.Get().GetName().ToLower()}.json", true, true);

            builder.AddEnvironmentVariables();

            return builder;
        }

        #endregion Mocks
    }
}