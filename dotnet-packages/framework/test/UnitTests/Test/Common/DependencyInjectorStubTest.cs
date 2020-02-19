using FluentAssertions;
using Framework.Test.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace Framework.Tests.UnitTests.Test.Common
{
    public class DependencyInjectorStubTest
    {
        #region Key

        [Fact]
        public void Get_Valid()
        {
            // arrange

            // act
            var provider = Get();

            // assert
            provider.Should().NotBeNull();
        }

        [Fact]
        public void Get_With_Configure()
        {
            // arrange

            // act
            var provider = Get((s, c) =>
            {
                s.AddScoped<Test>();
            });

            // assert
            provider.Should().NotBeNull();
            var service = provider.GetService<Test>();
            service.Should().NotBeNull();
        }

        [Fact]
        public void Get_With_ConfigData()
        {
            // arrange
            var configData = GetConfigData();
            var result = string.Empty;

            // act
            var provider = Get((s, c) => { result = c.GetValue<string>("KEY"); }, configData: configData);

            // assert
            provider.Should().NotBeNull();
            result.Should().Be("VALUE");
        }

        #endregion Key

        public static IServiceProvider Get(Action<IServiceCollection, IConfiguration> configure = null, Dictionary<string, string> configData = null)
        {
            return DependencyInjectorStub.Get(configure, configData);
        }

        #region Mocks

        public static Dictionary<string, string> GetConfigData()
        {
            return new Dictionary<string, string>() { { "KEY", "VALUE" } };
        }

        public class Test
        {
        }

        #endregion Mocks
    }
}