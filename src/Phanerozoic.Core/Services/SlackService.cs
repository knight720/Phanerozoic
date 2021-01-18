using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Services.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Phanerozoic.Core.Services
{
    public class SlackService : ISlackService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SlackService(IServiceProvider serviceProvider)
        {
            this._httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        }

        public async Task SendAsync(string webHookUrl, string slackMessageJson)
        {
            var httpClient = this._httpClientFactory.CreateClient();

            var content = new StringContent(slackMessageJson, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, webHookUrl);
            request.Content = content;

            var response = httpClient.SendAsync(request).Result;

            Console.WriteLine($"Slack Response: {response.StatusCode.ToString()}");
        }
    }
}