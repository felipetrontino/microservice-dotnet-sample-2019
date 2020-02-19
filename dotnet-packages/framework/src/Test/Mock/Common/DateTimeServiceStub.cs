using Framework.Core.Common;
using System;

namespace Framework.Test.Mock.Common
{
    public class DateTimeServiceStub : IDateTimeService
    {
        public static IDateTimeService Create() => new DateTimeServiceStub();

        public DateTime Now()
        {
            return DateTime.UtcNow.Date;
        }
    }
}