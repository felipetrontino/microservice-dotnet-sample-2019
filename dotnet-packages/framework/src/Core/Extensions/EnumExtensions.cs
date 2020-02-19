using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Framework.Core.Common;

namespace Framework.Core.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<string> GetNames(this Enum value)
        {
            var infoAttr = value.GetAttributes<EnumInfoAttribute>();

            if (infoAttr?.AlternativeNames != null)
            {
                foreach (var name in infoAttr.AlternativeNames)
                {
                    if (!string.IsNullOrWhiteSpace(name))
                        yield return name;
                }
            }

            yield return value.ToString();
        }

        private static TAttribute GetAttributes<TAttribute>(this Enum value)
            where TAttribute : class
        {
            return value.GetType().GetMember(value.ToString()).First().GetCustomAttributes(typeof(TAttribute), false).OfType<TAttribute>().FirstOrDefault();
        }

        public static string GetName(this Enum value)
        {
            return GetNames(value).First();
        }

        public static string GetDescription(this Enum value)
        {
            var attr = value.GetAttributes<DescriptionAttribute>();
            return attr != null && !string.IsNullOrWhiteSpace(attr.Description) ? attr.Description : value.ToString();
        }
    }
}