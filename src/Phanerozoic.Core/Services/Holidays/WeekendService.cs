using System;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Holidays
{
    public class WeekendService : IHolidayService
    {
        public bool IsHoliday(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Saturday => true,
                DayOfWeek.Sunday => true,
                _ => false,
            };
        }
    }
}