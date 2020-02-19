using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Framework.Core.Extensions
{
    public static class RegexExtensions
    {
        public static string RemoveDiacritics(this string value)
        {
            if (value == null) return null;

            var xNormalized = value.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in xNormalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string RemoveNumeric(this string value)
        {
            return value == null ? null : Regex.Replace(value, "\\d", "");
        }

        public static string RemoveAlphabetic(this string value)
        {
            return value == null ? null : Regex.Replace(value, "\\D", "");
        }

        public static string RemoveSpaces(this string value)
        {
            return value == null ? null : Regex.Replace(value, "\\s+", "");
        }

        public static string RemoveDoubleSpaces(this string value)
        {
            return value == null ? null : Regex.Replace(value, "\\s+", " ");
        }
    }
}