using System;

namespace Framework.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static long GetTotalMinutes(this DateTime value, DateTime endTime)
        {
            return (long)Math.Round((endTime - value).TotalMinutes, MidpointRounding.AwayFromZero);
        }

        public static DateTime TrimMilliseconds(this DateTime value) => new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, 0);
    }
}