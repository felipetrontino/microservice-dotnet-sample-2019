using Framework.Core.Extensions;
using System;
using System.Linq;

namespace Framework.Core.Helpers
{
    public static class EnumHelper
    {
        public static TEnum ParseTo<TEnum>(string value, TEnum defaultValue = default)
            where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;

            if (Enum.TryParse(value, true, out TEnum ret))
            {
                return ret;
            }

            var values = Enum.GetValues(typeof(TEnum));

            foreach (var item in values)
            {
                var isValid = ((Enum)item).GetNames().Any(x => x.Equals(value, StringComparison.InvariantCultureIgnoreCase));

                if (isValid)
                    return (TEnum)item;
            }

            return defaultValue;
        }
    }
}