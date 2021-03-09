using System;
using System.Net.Http;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Holidays
{
    public class GovementHoliday : IHolidayService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GovementHoliday(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public bool IsHoliday(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}