using System;
using FluentAssertions;
using Framework.Core.Common;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Common
{
    public class DateTimeServiceTest
    {
        [Fact]
        public void Now_Valid()
        {
            // arrange

            // act
            var result = Now();

            // assert
            var resultExpected = DateTime.UtcNow;
            result.Should().BeCloseTo(resultExpected, TimeSpan.FromMilliseconds(500));
        }

        private static DateTime Now()
        {
            var svc = new DateTimeService();
            return svc.Now();
        }
    }
}