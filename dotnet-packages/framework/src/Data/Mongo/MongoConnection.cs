using Framework.Core.Config;
using Framework.Core.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Framework.Data.Mongo
{
    public static class MongoConnection
    {
        private static readonly object Lock = new object();

        private static bool _isRegistered;
        private static readonly Dictionary<string, IMongoDatabase> Databases = new Dictionary<string, IMongoDatabase>();

        public static IMongoDatabase GetDatabase(string connectionString)
        {
            var conn = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

            lock (Lock)
            {
                Databases.TryGetValue(conn, out var database);

                if (database != null) return database;

                var url = new MongoUrlBuilder(conn);

                if (url.DatabaseName == null)
                    url.DatabaseName = $"{Configuration.Stage.Get().GetName().ToUpper()}_{Configuration.AppName.Get().ToUpper()}";

                var client = new MongoClient(MongoClientSettings.FromUrl(url.ToMongoUrl()));
                database = client.GetDatabase(url.DatabaseName);

                Register();

                Databases[conn] = database;

                return database;
            }
        }

        private static void Register()
        {
            var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("IgnoreElements", conventionPack, type => true);

            if (!_isRegistered)
            {
                BsonSerializer.RegisterSerializer(typeof(DateTime), new UtcDateTimeSerializer());
                _isRegistered = true;
            }
        }
    }
}