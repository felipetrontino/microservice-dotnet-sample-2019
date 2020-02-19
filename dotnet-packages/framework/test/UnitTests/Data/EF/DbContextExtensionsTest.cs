using FluentAssertions;
using Framework.Core.Common;
using Framework.Core.Config;
using Framework.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Framework.Tests.UnitTests.Data.EF
{
    public class DbContextExtensionsTest
    {
        public DbContextExtensionsTest()
        {
            Configuration.Stage.Set(EnvironmentType.Development);
            Configuration.AppName.Set("TEST");
        }

        #region AddDb

        [Fact]
        public void AddDb_Conn_Without_InitialCatalog()
        {
            // arrange
            var service = new ServiceCollection();
            var conn = GetConnWithoutCatalog();

            // act
            var result = AddDb<Data>(service, conn);

            // assert
            var resultExpected = GetConn();

            var provider = result.BuildServiceProvider();
            var db = provider.GetService<Data>();
            var dbConn = db.Database.GetDbConnection();
            dbConn.ConnectionString.Should().Be(resultExpected);
        }

        [Fact]
        public void AddDb_Conn_With_InitialCatalog()
        {
            // arrange
            var service = new ServiceCollection();
            var conn = GetConn("A");

            // act
            var result = AddDb<Data>(service, conn);

            // assert
            var resultExpected = conn;

            var provider = result.BuildServiceProvider();
            var db = provider.GetService<Data>();
            var dbConn = db.Database.GetDbConnection();
            dbConn.ConnectionString.Should().Be(resultExpected);
        }

        [Fact]
        public void AddDb_Conn_Null()
        {
            // arrange
            var service = new ServiceCollection();
            var conn = GetConnNull();

            // act
            var result = AddDb<Data>(service, conn);

            // assert
            var resultExpected = GetConnDefault();

            var provider = result.BuildServiceProvider();
            var db = provider.GetService<Data>();
            var dbConn = db.Database.GetDbConnection();
            dbConn.ConnectionString.Should().Be(resultExpected);
        }

        [Fact]
        public void AddDb_Conn_Empty()
        {
            // arrange
            var service = GetService();
            var conn = GetConnEmpty();

            // act
            var result = AddDb<Data>(service, conn);

            // assert
            var resultExpected = GetConnDefault();

            var provider = result.BuildServiceProvider();
            var db = provider.GetService<Data>();
            var dbConn = db.Database.GetDbConnection();
            dbConn.ConnectionString.Should().Be(resultExpected);
        }

        #endregion AddDb

        private static IServiceCollection AddDb<TContext>(IServiceCollection serviceCollection, string connectionString)
            where TContext : BaseDbContext
        {
            return serviceCollection.AddDb<TContext>(connectionString);
        }

        #region Mocks

        public static IServiceCollection GetService()
        {
            return new ServiceCollection();
        }

        private static string GetConnNull()
        {
            return null;
        }

        private static string GetConnEmpty()
        {
            return string.Empty;
        }

        private static string GetConn(string name = "TEST")
        {
            return $"Data Source=.;Initial Catalog=DEV_{name};Integrated Security=True";
        }

        private static string GetConnWithoutCatalog()
        {
            return "Data Source=.;Integrated Security=True";
        }

        private static string GetConnDefault()
        {
            return "Initial Catalog=DEV_TEST";
        }

        public class Data : BaseDbContext
        {
            public Data(DbContextOptions options)
                : base(options)
            {
            }
        }

        #endregion Mocks
    }
}