using Framework.Core.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Test.Common
{
    public static class MockBuilder
    {
        public static string Key => Guid.NewGuid().ToString();

        private static ConcurrentDictionary<string, int> _increments = new ConcurrentDictionary<string, int>();
        private static ConcurrentDictionary<string, Guid> _ids = new ConcurrentDictionary<string, Guid>();

        public static DateTime Date => DateTime.UtcNow.Date;

        public static string[] Keys(int qtd)
        {
            var ret = new string[qtd];

            for (var i = 0; i < qtd; i++)
            {
                ret[i] = Key;
            }

            return ret;
        }

        public static Guid GetId(string key = null)
        {
            if (Guid.TryParse(key, out var result))
                return result;

            return Guid.Empty;
        }

        public static Guid GetIdByValue(object value)
        {
            var hash = HashHelper.Create(value);

            var isValid = _ids.TryGetValue(hash, out var id);

            if (isValid) return id;

            id = Guid.NewGuid();
            _ids[hash] = id;

            return id;
        }

        public static int GetIncrement(string key = null)
        {
            if (key == null) return 0;

            var isValid = _increments.TryGetValue(key, out var ret);

            if (isValid) return ret;

            ret = _increments.Values.LastOrDefault() + 1;
            _increments[key] = ret;

            return ret;
        }

        public static void ResetBags()
        {
            _increments = new ConcurrentDictionary<string, int>();
            _ids = new ConcurrentDictionary<string, Guid>();
        }

        public static List<T> List<T>(params T[] values)
        {
            return values != null ? values.ToList() : ListEmpty<T>();
        }

        public static List<T> ListEmpty<T>() => new List<T>();

        public static Exception GetException(string message = null)
        {
            return new Exception(message ?? "ERROR");
        }
    }
}