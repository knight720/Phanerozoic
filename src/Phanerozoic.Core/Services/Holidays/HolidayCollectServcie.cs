using System;
using System.Collections.Generic;
using System.Linq;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Holidays
{
    public class HolidayCollectServcie : IHolidayService
    {
        private readonly IList<IHolidayService> _providerList = new List<IHolidayService>();

        public bool IsHoliday(DateTime date)
        {
            return this._providerList.Any(i => i.IsHoliday(date));
        }

        public void Add(IHolidayService holidayService)
        {
            this._providerList.Add(holidayService);
        }
    }
}