using Framework.Core.Config;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsFeatureEnabled(this IConfiguration value, string name)
        {
            return value[$"{Configuration.SectionNames.Feature}:{name}"].ToBoolean();
        }

        public static Dictionary<string, string> ToDictionaryConfig<T>(this T obj, params string[] sectionNames)
        {
            var ret = new Dictionary<string, string>();

            if (obj == null) return ret;

            var xSection = sectionNames != null && sectionNames.Any() ? string.Join(":", sectionNames) + ":" : string.Empty;

            if (obj is IDictionary dic)
            {
                foreach (var key in dic.Keys)
                {
                    var name = $"{xSection}{key}".ToLower();

                    var value = dic[key];
                    ret.Add(name, value?.ToString());
                }
            }
            else
            {
                var properties = typeof(T).GetProperties();

                foreach (var prop in properties)
                {
                    var name = $"{xSection}{prop.Name}".ToLower();
                    ret.Add(name, prop.GetValue(obj)?.ToString());
                }
            }

            return ret;
        }
    }
}