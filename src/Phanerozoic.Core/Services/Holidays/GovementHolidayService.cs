using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Holidays
{
    public class GovementHolidayService : IHolidayService
    {
        private readonly HttpClient _httpClient;

        public GovementHolidayService(IHttpClientFactory httpClientFactory)
        {
            this._httpClient = httpClientFactory.CreateClient();
        }

        public bool IsHoliday(DateTime date)
        {
            var url = "https://data.ntpc.gov.tw/api/datasets/308DCD75-6434-45BC-A95F-584DA4FED251/json?page=1&size=950";
            var httpResponseMessage = this._httpClient.GetAsync(url).GetAwaiter().GetResult();
            Console.WriteLine($"Open API StatusCode: {httpResponseMessage.StatusCode}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var holidayList = JsonSerializer.Deserialize<IList<Holiday>>(content);
                var holiday = holidayList.FirstOrDefault(i => i.Date.Equals(date.ToString("yyyy/M/d")));
                if (holiday == null)
                {
                    return false;
                }

                Console.WriteLine(holiday.ToString());
                return true;
            }
            return false;
        }

        private class Holiday
        {
            [JsonPropertyName("date")]
            public string Date { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("isHoliday")]
            public string IsHoliday { get; set; }

            [JsonPropertyName("holidayCategory")]
            public string HolidayCategory { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            public override string ToString()
            {
                var list = new string[] { Date, Name, IsHoliday, HolidayCategory, Description };
                list = list.Where(i => string.IsNullOrWhiteSpace(i) == false).ToArray();
                return string.Join(", ", list);
            }
        }
    }
}