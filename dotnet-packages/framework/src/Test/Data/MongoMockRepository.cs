using Framework.Core.Entities;
using Framework.Data.Mongo;
using Framework.Test.Common;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Test.Data
{
    public class MongoMockRepository<TDataContext> : IMockRepository<TDataContext>
        where TDataContext : BaseMongoContext
    {
        private readonly TDataContext _db;
        private List<Func<Task>> _commands = new List<Func<Task>>();

        public MongoMockRepository()
        {
            MockBuilder.ResetBags();

            var conn = MongoInMemory.GetConnectionString(Guid.NewGuid().ToString());
            var db = (TDataContext)Activator.CreateInstance(typeof(TDataContext), conn);

            _db = db;
        }

        public void Add<T>(T e)
            where T : class, IEntity
        {
            _commands.Add(() => _db.Database.GetCollection<T>().AddAsync(e));
        }

        public void Commit()
        {
            var commands = _commands.ToList();
            _commands = new List<Func<Task>>();

            foreach (var command in commands)
            {
                command().Wait();
            }
        }

        public TDataContext GetContext()
        {
            return _db;
        }

        public IQueryable<T> Query<T>()
            where T : class, IEntity
        {
            return _db.Database.GetCollection<T>().AsQueryable();
        }

        public void Remove<T>(T e)
            where T : class, IEntity
        {
            _commands.Add(() => _db.Database.GetCollection<T>().RemoveAsync(e));
        }
    }
}