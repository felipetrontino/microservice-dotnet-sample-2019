using Framework.Core.Entities;
using Framework.Data.EF;
using Framework.Test.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Framework.Test.Data
{
    public class EfMockRepository<TDataContext> : IMockRepository<TDataContext>
        where TDataContext : BaseDbContext
    {
        private readonly DbContextOptions _options;
        private TDataContext _db;

        public EfMockRepository()
        {
            MockBuilder.ResetBags();

            var builder = new DbContextOptionsBuilder<TDataContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _options = builder.Options;

            _db = GetDbContext(_options);
        }

        public void Add<T>(T e)
            where T : class, IEntity
        {
            _db.Add(e);
        }

        public void Commit()
        {
            _db.SaveChanges();
            _db = GetDbContext(_options);
        }

        public IQueryable<T> Query<T>()
            where T : class, IEntity
        {
            return _db.Set<T>();
        }

        public void Remove<T>(T e)
            where T : class, IEntity
        {
            _db.Remove(e);
        }

        private static TDataContext GetDbContext(DbContextOptions options)
        {
            var db = (TDataContext)Activator.CreateInstance(typeof(TDataContext), options);
            db.EnsureSeedData();

            return db;
        }

        public TDataContext GetContext()
        {
            return GetDbContext(_options);
        }
    }
}