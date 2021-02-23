using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Slacks
{
    public class SlackService : ISlackService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SlackService(IServiceProvider serviceProvider)
        {
            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        }

        public async Task SendAsync(string webHookUrl, string slackMessageJson)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var content = new StringContent(slackMessageJson, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, webHookUrl);
            request.Content = content;

            var response = httpClient.SendAsync(request).Result;

            Console.WriteLine($"Slack Response: {response.StatusCode.ToString()}");
        }
    }
}