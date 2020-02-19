using FluentAssertions;
using Framework.Data.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Framework.Tests.UnitTests.Data.EF
{
    public class DesignTimeDbContextFactoryTest
    {
        [Fact]
        public void DesignTimeDbContextFactory_Schema_Valid()
        {
            // arrange
            var factory = GetFactory();

            // act
            var result = factory.Schema;

            // assert
            var resultExpected = GetSchema();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void DesignTimeDbContextFactory_ConnectionString_Valid()
        {
            // arrange
            var factory = GetFactory();

            // act
            var result = factory.ConnectionString;

            // assert
            var resultExpected = GetConnectionStringDefault();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void DesignTimeDbContextFactory_ConnectionString_Overrided()
        {
            // arrange
            var factory = GetFactoryOverrided();

            // act
            var result = factory.ConnectionString;

            // assert
            var resultExpected = GetConnectionString();
            result.Should().Be(resultExpected);
        }

        [Fact]
        public void DesignTimeDbContextFactory_GetOptionsBuilder_Valid()
        {
            // arrange
            var factory = GetFactory();

            // act
            var result = factory.GetOptionsBuilder();

            // assert
            var resultExpected = GetOptionsBuilderDefault();
            result.Options.ContextType.Should().Be(resultExpected.Options.ContextType);
        }

        [Fact]
        public void DesignTimeDbContextFactory_GetOptionsBuilder_Overrided()
        {
            // arrange
            var factory = GetFactoryOverrided();

            // act
            var result = factory.GetOptionsBuilder();

            // assert
            var resultExpected = GetOptionsBuilder();
            result.Options.ContextType.Should().Be(resultExpected.Options.ContextType);
        }

        [Fact]
        public void DesignTimeDbContextFactory_CreateDbContext_Valid()
        {
            // arrange
            var factory = GetFactory();

            // act
            var result = factory.CreateDbContext(null);

            // assert
            result.Should().NotBeNull();
        }

        #region Mocks

        private static MyDesignTimeDbContextFactory GetFactory()
        {
            return new MyDesignTimeDbContextFactory();
        }

        private static MyDesignTimeDbContextFactoryOverrided GetFactoryOverrided()
        {
            return new MyDesignTimeDbContextFactoryOverrided();
        }

        private static string GetSchema()
        {
            return nameof(DesignTimeDbContextFactoryTest);
        }

        private static string GetConnectionStringDefault()
        {
            return "Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True;";
        }

        private static string GetConnectionString()
        {
            return "Data Source=.;Initial Catalog=MyDb;Integrated Security=True;";
        }

        private static DbContextOptionsBuilder GetOptionsBuilderDefault()
        {
            return new DbContextOptionsBuilder<Db>();
        }

        private static DbContextOptionsBuilder GetOptionsBuilder()
        {
            return new DbContextOptionsBuilder<Db2>();
        }

        public class MyDesignTimeDbContextFactory : DesignTimeDbContextFactory<Db>
        {
            public override string Schema => nameof(DesignTimeDbContextFactoryTest);
        }

        public class MyDesignTimeDbContextFactoryOverrided : DesignTimeDbContextFactory<Db>
        {
            public override string Schema => nameof(DesignTimeDbContextFactoryTest);

            public override string ConnectionString => GetConnectionString();

#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members

            public override DbContextOptionsBuilder GetOptionsBuilder()
#pragma warning restore S3218 // Inner class members should not shadow outer class "static" or type members
            {
                base.GetOptionsBuilder();

                return new DbContextOptionsBuilder<Db2>();
            }
        }

        public class Db : DbContext
        {
            public Db(DbContextOptions options)
                : base(options)
            {
            }
        }

        public class Db2 : DbContext
        {
            public Db2(DbContextOptions options)
                : base(options)
            {
            }
        }

        #endregion Mocks
    }
}