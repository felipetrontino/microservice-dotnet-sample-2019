using System.Collections.Generic;

namespace Framework.Core.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (!dictionary.TryGetValue(key, out var retValue))
                retValue = defaultValue;

            return retValue;
        }
    }
}