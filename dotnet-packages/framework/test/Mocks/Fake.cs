using System;

namespace Framework.Tests.Mocks
{
    public static class Fake
    {
        public static string StringNull => null;

        public static string StringEmpty => string.Empty;

        public static string StringWhiteSpace => " ";

        public static DateTime GetDate()
        {
            return DateTime.MinValue;
        }

        public static decimal GetDecimal()
        {
            return 1.5M;
        }

        public static int GetInteger()
        {
            return 1;
        }

        public static double GetDouble()
        {
            return 1000.5D;
        }

        public static bool GetBoolean()
        {
            return true;
        }

        public static string GetName(string key = null)
        {
            return $"{key}_NAME";
        }

        public static string GetUserName(string key = null)
        {
            return $"{key}_USER";
        }

        public static string GetPassword(string key = null)
        {
            return $"{key}_PASSWORD";
        }
    }
}