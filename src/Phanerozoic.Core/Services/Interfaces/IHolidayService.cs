using System;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface IHolidayService
    {
        bool IsHoliday(DateTime date);
    }
}