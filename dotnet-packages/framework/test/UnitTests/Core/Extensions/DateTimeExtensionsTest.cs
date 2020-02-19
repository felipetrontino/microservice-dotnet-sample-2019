using FluentAssertions;
using Framework.Core.Extensions;
using Framework.Tests.Mocks;
using System;
using Xunit;

namespace Framework.Tests.UnitTests.Core.Extensions
{
    public class DateTimeExtensionsTest
    {
        #region GetTotalMinutes

        [Fact]
        public void GetTotalMinutes_Valid()
        {
            // arrange
            var startDate = Fake.GetDate();
            var endDate = startDate.AddMinutes(1);

            // act
            var minutes = GetTotalMinutes(startDate, endDate);

            // assert
            minutes.Should().Be(1);
        }

        [Fact]
        public void GetTotalMinutes_HalfWay_Near_Zero()
        {
            // arrange
            var startDate = Fake.GetDate();
            var endDate = startDate.AddSeconds(29);

            // act
            var minutes = GetTotalMinutes(startDate, endDate);

            // assert
            minutes.Should().Be(0);
        }

        [Fact]
        public void GetTotalMinutes_HalfWay_Away_Zero()
        {
            // arrange
            var startDate = Fake.GetDate();
            var endDate = startDate.AddSeconds(30);

            // act
            var minutes = GetTotalMinutes(startDate, endDate);

            // assert
            minutes.Should().Be(1);
        }

        #endregion GetTotalMinutes

        #region TrimMilliseconds

        [Fact]
        public void TrimMilliseconds_Valid()
        {
            // arrange
            var date = DateTime.UtcNow;

            // act
            var result = TrimMilliseconds(date);

            // assert
            var dateExpected = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
            dateExpected.Should().Be(result);
        }

        #endregion TrimMilliseconds

        private static long GetTotalMinutes(DateTime value, DateTime endTime)
        {
            return value.GetTotalMinutes(endTime);
        }

        private static DateTime TrimMilliseconds(DateTime value)
        {
            return value.TrimMilliseconds();
        }
    }
}