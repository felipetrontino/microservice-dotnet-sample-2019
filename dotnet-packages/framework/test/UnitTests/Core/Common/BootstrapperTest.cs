using FluentAssertions;
using Framework.Core.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Common
{
    public class BootstrapperTest
    {
        public BootstrapperTest()
        {
        }

        [Fact]
        public void RegisterServices_Valid()
        {
            // arrange
            var services = GetServices();
            var config = GetConfig();

            // act
            RegisterServices(services, config);

            // assert
            var servicesExpected = GetServices(config);
            services.Count.Should().Be(servicesExpected.Count);
            var provider = services.BuildServiceProvider();
            var configExpected = provider.GetService<IConfiguration>();
            config.Should().BeEquivalentTo(configExpected);
        }

        [Fact]
        public void RegisterServices_With_Register()
        {
            // arrange
            var services = GetServices();
            var config = GetConfig();
            var executed = false;

            // act
            RegisterServices(services, config, (s, c) => { executed = true; });

            // assert
            var servicesExpected = GetServices(config);
            services.Count.Should().Be(servicesExpected.Count);
            var provider = services.BuildServiceProvider();
            var configExpected = provider.GetService<IConfiguration>();
            config.Should().BeEquivalentTo(configExpected);

            executed.Should().BeTrue();
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration config, Action<IServiceCollection, IConfiguration> register = null)
        {
            Bootstrapper.RegisterServices(services, config, register);
        }

        #region Mocks

        private static IServiceCollection GetServices()
        {
            return new ServiceCollection();
        }

        private static IServiceCollection GetServices(IConfiguration config)
        {
            var services = new ServiceCollection();
            services.AddScoped<IConfiguration>(x => config);

            return services;
        }

        private static IConfiguration GetConfig()
        {
            return new ConfigurationBuilder().Build();
        }

        #endregion Mocks
    }
}