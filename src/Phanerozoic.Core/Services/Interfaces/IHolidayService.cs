using System;

namespace Phanerozoic.Core.Services.Interfaces
{
    internal interface IHolidayService
    {
        bool IsHoliday(DateTime date);
    }
}