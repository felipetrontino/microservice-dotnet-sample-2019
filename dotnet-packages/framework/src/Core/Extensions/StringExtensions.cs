using System;
using System.IO;
using System.Text;

namespace Framework.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsEmptyTrim(this string value) => string.IsNullOrWhiteSpace(value);

        public static string DiscardLeadingZeroes(this string value)
        {
            return long.TryParse(value, out var n) ? n.ToString() : value;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string ToPascalCase(this string value)
        {
            if (value == null) return value;

            if (value.Length < 2) return value.ToUpper();

            var words = value.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            var sb = new StringBuilder();

            foreach (var word in words)
            {
                sb.Append(word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower());
            }

            return sb.ToString();
        }

        public static string ToCamelCase(this string value)
        {
            if (value == null) return value;

            if (value.Length < 2) return value.ToLower();

            var words = value.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            var sb = new StringBuilder();
            sb.Append(words[0].ToLower());

            for (var i = 1; i < words.Length; i++)
            {
                sb.Append(words[i].Substring(0, 1).ToUpper() + words[i].Substring(1).ToLower());
            }

            return sb.ToString();
        }

        public static byte[] ToBytes(this string value)
        {
            return value == null ? null : Encoding.UTF8.GetBytes(value);
        }

        public static Stream ToStream(this string value)
        {
            return value == null ? null : new MemoryStream(ToBytes(value));
        }
    }
}