using System;

namespace Framework.Core.Common
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}