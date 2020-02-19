using Framework.Core.Entities;
using System.Linq;

namespace Framework.Test.Data
{
    public interface IMockRepository<out TContext>
    {
        void Add<T>(T e) where T : class, IEntity;

        void Remove<T>(T e) where T : class, IEntity;

        IQueryable<T> Query<T>() where T : class, IEntity;

        void Commit();

        TContext GetContext();
    }
}