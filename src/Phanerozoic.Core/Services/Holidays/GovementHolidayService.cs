using System;
using System.Net.Http;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Holidays
{
    public class GovementHolidayService : IHolidayService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GovementHolidayService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public bool IsHoliday(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}