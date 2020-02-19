using Framework.Core.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Data.Mongo
{
    public static class MongoExtensions
    {
        public static IMongoCollection<T> GetCollection<T>(this IMongoDatabase session)
        {
            var attrs = typeof(T).GetCustomAttributes(typeof(CollectionNameAttribute), false).OfType<CollectionNameAttribute>().FirstOrDefault();
            var collectionName = attrs?.Name ?? typeof(T).Name;

            return session.GetCollection<T>(collectionName);
        }

        public static IMongoQueryable<T> Query<T>(this IMongoDatabase session)
        {
            return session.GetCollection<T>().AsQueryable();
        }

        public static IMongoQueryable<T> Query<T>(this IMongoDatabase session, string name)
        {
            return session.GetCollection<T>(name).AsQueryable();
        }

        public static async Task AddAsync<T>(this IMongoCollection<T> collection, T value, CancellationToken cancellationToken = default)
        {
            await collection.InsertOneAsync(value, cancellationToken: cancellationToken);
        }

        public static async Task AddRangeAsync<T>(this IMongoCollection<T> collection, IEnumerable<T> values, CancellationToken cancellationToken = default)
        {
            await collection.InsertManyAsync(values, cancellationToken: cancellationToken);
        }

        public static async Task AddOrUpdateAsync<T>(this IMongoCollection<T> collection, T value, CancellationToken cancellationToken = default)
            where T : IEntity
        {
            await collection.ReplaceOneAsync(x => x.Id == value.Id, value, new ReplaceOptions { IsUpsert = true }, cancellationToken);
        }

        public static async Task AddOrUpdateAsync<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> filter, T value, CancellationToken cancellationToken = default)
            where T : IEntity
        {
            await collection.FindOneAndReplaceAsync(filter, value, new FindOneAndReplaceOptions<T, T> { IsUpsert = true }, cancellationToken);
        }

        public static async Task UpdateAsync<T>(this IMongoCollection<T> collection, T value, CancellationToken cancellationToken = default)
            where T : IEntity
        {
            await collection.ReplaceOneAsync(x => x.Id == value.Id, value, cancellationToken: cancellationToken);
        }

        public static async Task UpdateAsync<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> filter, T value, CancellationToken cancellationToken = default)
            where T : IEntity
        {
            await collection.ReplaceOneAsync(filter, value, cancellationToken: cancellationToken);
        }

        public static async Task RemoveAsync<T>(this IMongoCollection<T> collection, T value, CancellationToken cancellationToken = default)
           where T : IEntity
        {
            await collection.DeleteOneAsync(x => x.Id == value.Id, cancellationToken);
        }

        public static async Task RemoveAsync<T>(this IMongoCollection<T> collection, Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            where T : IEntity
        {
            await collection.DeleteManyAsync(filter, cancellationToken);
        }
    }
}