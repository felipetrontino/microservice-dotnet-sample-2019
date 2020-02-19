using FluentAssertions;
using Framework.Core.Common;
using Framework.Test.Mock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Common
{
    public class ConsoleBootstrapTest
    {
        public ConsoleBootstrapTest()
        {
        }

        [Fact]
        public void RunAsync_Valid()
        {
            // arrange
            var executed = false;

            // act
            var result = RunAsync<Startup>(x =>
            {
                executed = true;
                return Task.CompletedTask;
            });

            // assert
            result.Should().Be(0);
            executed.Should().BeTrue();
        }

        [Fact]
        public void RunAsync_Exception()
        {
            // arrange
            ConsoleStub.Create("1");
            // act
            var result = RunAsync<Startup>(x =>
            {
                throw new Exception();
            });

            // assert
            result.Should().Be(1);
        }

        [Fact]
        public void RunAsync_Without_Configure()
        {
            // arrange
            var executed = false;

            // act
            var result = RunAsync<StartupWithoutConfigure>(x =>
            {
                executed = true;
                return Task.CompletedTask;
            });

            // assert
            result.Should().Be(0);
            executed.Should().BeTrue();
        }

        private static int RunAsync<TStartup>(Func<IServiceProvider, Task> execute)
            where TStartup : class
        {
            return ConsoleBootstrap.RunAsync<TStartup>(execute).GetAwaiter().GetResult();
        }

        #region Mocks

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services, IConfiguration config)
            {
                if (services == null) throw new ArgumentNullException(nameof(services));
                if (config == null) throw new ArgumentNullException(nameof(config));
                // Ignore
            }
        }

        public class StartupWithoutConfigure
        {
        }

        #endregion Mocks
    }
}