using FluentAssertions;
using Framework.Core.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Common
{
    public class BaseStartupTest
    {
        [Fact]
        public void Constructor_Valid()
        {
            // arrange

            // act
            var result = Constructor();

            // assert
            var resultExpected = GetObj();
            result.Should().BeEquivalentTo(resultExpected);
        }

        [Fact]
        public void ConfigureServices_Valid()
        {
            // arrange
            var obj = GetObj();

            // act
            ConfigureServices(obj);

            // assert
            var resultExpected = GetObj();
            resultExpected.Executed = true;
            obj.Should().BeEquivalentTo(resultExpected);
        }

        private static Startup Constructor()
        {
            return new Startup();
        }

        private static void ConfigureServices(BaseStartup obj)
        {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder().Build();

            obj.ConfigureServices(services, config);
        }

        #region Mocks

        private static Startup GetObj()
        {
            return new Startup();
        }

        public class Startup : BaseStartup
        {
            public bool Executed { get; set; }

            protected override void RegisterService(IServiceCollection services, IConfiguration config)
            {
                base.RegisterService(services, config);

                Executed = true;
            }
        }

        #endregion Mocks
    }
}