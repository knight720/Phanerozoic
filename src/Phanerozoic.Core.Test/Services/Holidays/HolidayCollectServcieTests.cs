using System;
using FluentAssertions;
using Xunit;

namespace Phanerozoic.Core.Services.Holidays.Tests
{
    public class HolidayCollectServcieTests
    {
        [Fact()]
        public void 沒有Provider時一律回傳False()
        {
            //// Arrange
            var date = new DateTime(2021, 4, 10);

            var target = new HolidayCollectServcie();

            //// Act
            var actual = target.IsHoliday(date);

            //// Assert
            actual.Should().BeFalse();
        }

        [Fact()]
        public void 使用WeekendProvider星期六回傳True()
        {
            //// Arrange
            var date = new DateTime(2021, 4, 10);

            var target = new HolidayCollectServcie();
            target.Add(new WeekendService());
            //// Act
            var actual = target.IsHoliday(date);

            //// Assert
            actual.Should().BeTrue();
        }

        [Fact()]
        public void 使用WeekendProvider星期五回傳False()
        {
            //// Arrange
            var date = new DateTime(2021, 4, 9);

            var target = new HolidayCollectServcie();
            target.Add(new WeekendService());
            //// Act
            var actual = target.IsHoliday(date);

            //// Assert
            actual.Should().BeFalse();
        }
    }
}