using Framework.Core.Entities;
using System;
using System.Linq;

namespace Framework.Test.Common
{
    public class MockBuilder<TBuilder, TEntity> : IMockBuilder<TBuilder, TEntity>
        where TBuilder : class, IMockBuilder<TBuilder, TEntity>
        where TEntity : class, new()
    {
        public string Key { get; set; }

        public TEntity Value { get; set; }

        public static TBuilder Create(string key = null)
        {
            var ret = Activator.CreateInstance<TBuilder>();
            ret.Key = key;
            ret.Value = CreateModel<TEntity>(ret.Key);

            return ret;
        }

        public TEntity Build()
        {
            return Value;
        }

        protected static T CreateModel<T>(string key)
            where T : class, new()
        {
            var ret = new T();

            var idProp = typeof(T).GetProperties().FirstOrDefault(x => x.Name == nameof(Entity.Id));

            if (idProp == null) return ret;

            var id = key != null ? MockBuilder.GetId(key) : Guid.Empty;
            idProp.SetValue(ret, id);

            return ret;
        }
    }
}