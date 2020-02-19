using FluentAssertions;
using Framework.Core.Entities;
using Framework.Data.Mongo;
using Framework.Test.Common;
using Framework.Test.Data;
using MongoDB.Driver;
using System;
using System.Linq;
using Xunit;

namespace Framework.Tests.UnitTests.Test.Data
{
    [Collection(nameof(DatabaseFixture))]
    public class MongoMockRepositoryTest
    {
        [Fact]
        public void Constructor_Valid()
        {
            // arrange

            // act
            var repository = Get();

            // assert
            repository.Should().NotBeNull();
        }

        [Fact]
        public void Add_Valid()
        {
            // arrange
            var repository = Get();
            var e = GetEntity();

            // act
            repository.Add(e);

            // assert
            var entityContext = repository.Query<EntityTest>().FirstOrDefault();
            entityContext.Should().BeNull();

            var newContext = repository.GetContext();
            var entityNotSaved = newContext.Entities.AsQueryable().FirstOrDefault();
            entityNotSaved.Should().BeNull();
        }

        [Fact]
        public void Add_Commit_Valid()
        {
            // arrange
            var repository = Get();
            var e = GetEntity();

            // act
            repository.Add(e);
            repository.Commit();

            // assert
            var entityContext = repository.Query<EntityTest>().FirstOrDefault();
            entityContext.Should().NotBeNull();
            entityContext.Should().BeEquivalentTo(e);

            var newContext = repository.GetContext();
            var entityNotSaved = newContext.Entities.AsQueryable().FirstOrDefault();
            entityNotSaved.Should().NotBeNull();
            entityNotSaved.Should().BeEquivalentTo(e);
        }

        [Fact]
        public void Commit_Valid()
        {
            // arrange
            var repository = Get();

            // act
            repository.Commit();

            // assert
            var entityContext = repository.Query<EntityTest>().FirstOrDefault();
            entityContext.Should().BeNull();
        }

        [Fact]
        public void Query_Valid()
        {
            // arrange
            var repository = Get();
            var e = GetEntity();
            repository.Add(e);
            repository.Commit();

            // act
            var result = repository.Query<EntityTest>().FirstOrDefault();

            // assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(e);
        }

        [Fact]
        public void Remove_Valid()
        {
            // arrange
            var repository = Get();
            var e = GetEntity();
            repository.Add(e);
            repository.Commit();

            // act
            repository.Remove(e);

            // assert
            var entityContext = repository.Query<EntityTest>().FirstOrDefault();
            entityContext.Should().NotBeNull();
            entityContext.Should().BeEquivalentTo(e);

            var newContext = repository.GetContext();
            var entityNotSaved = newContext.Entities.AsQueryable().FirstOrDefault();
            entityNotSaved.Should().NotBeNull();
            entityNotSaved.Should().BeEquivalentTo(e);
        }

        [Fact]
        public void Remove_Commit_Valid()
        {
            // arrange
            var repository = Get();
            var e = GetEntity();
            repository.Add(e);
            repository.Commit();

            // act
            repository.Remove(e);
            repository.Commit();

            // assert
            var entityContext = repository.Query<EntityTest>().FirstOrDefault();
            entityContext.Should().BeNull();

            var newContext = repository.GetContext();
            var entityNotSaved = newContext.Entities.AsQueryable().FirstOrDefault();
            entityNotSaved.Should().BeNull();
        }

        [Fact]
        public void GetContext_Valid()
        {
            // arrange
            var repository = Get();

            // act
            var result = repository.GetContext();

            // assert
            var dbExpected = GetDb();
            result.Database.Settings.Should().BeEquivalentTo(dbExpected.Database.Settings);
        }

        #region Mocks

        public MongoMockRepository<Db> Get()
        {
            return new MongoMockRepository<Db>();
        }

        public EntityTest GetEntity()
        {
            return new EntityTest();
        }

        public BaseMongoContext GetDb()
        {
            var conn = MongoInMemory.GetConnectionString(Guid.NewGuid().ToString());
            return new Db(conn);
        }

        public class Db : BaseMongoContext
        {
            public Db(string connectionString) : base(connectionString)
            {
            }

            public IMongoCollection<EntityTest> Entities => Database.GetCollection<EntityTest>();
        }

        public class EntityTest : IEntity
        {
            public Guid Id { get; set; } = Guid.NewGuid();
        }

        #endregion Mocks
    }
}