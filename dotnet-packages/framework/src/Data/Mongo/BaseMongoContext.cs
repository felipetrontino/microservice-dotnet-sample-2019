using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Framework.Data.Mongo
{
    public abstract class BaseMongoContext : IMongoContext
    {
        protected BaseMongoContext(string connectionString)
        {
            Database = MongoConnection.GetDatabase(connectionString);
            Configuring();
        }

        public IMongoDatabase Database { get; }

        protected virtual void Configure()
        {
        }

        protected void Register<T>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>();
            }
        }

        private void Configuring()
        {
            Configure();
        }

        public IMongoCollection<T> GetCollection<T>(string name = null)
        {
            return name != null ? Database.GetCollection<T>(name) : Database.GetCollection<T>();
        }
    }
}