using System;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Phanerozoic.Core.Services.Tests
{
    public class SlackServiceTests
    {
        private readonly IServiceProvider _ServiceProvider;

        public SlackServiceTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient();
            this._ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact(Skip = "整合測試")]
        public void SendAsyncTest()
        {
            //// Arrange
            string webHookUrl = "";
            var data = new { text = "<@UserID|cal> bla bla" };
            var json = JsonSerializer.Serialize(data);

            //// Act
            var target = this.GetTarget();
            target.SendAsync(webHookUrl, json);

            //// Assert
            Assert.True(true);
        }

        private SlackService GetTarget()
        {
            return new SlackService(this._ServiceProvider);
        }
    }
}