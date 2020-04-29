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
        public void Test_Tag_User()
        {
            //// Arrange
            string webHookUrl = "";
            var data = new { text = "<@UserID|cal> bla bla `Ada` `Boy`" };
            var json = JsonSerializer.Serialize(data);

            //// Act
            var target = this.GetTarget();
            target.SendAsync(webHookUrl, json);

            //// Assert
            Assert.True(true);
        }

        [Fact(Skip = "整合測試")]
        public void Test_Tag_Group_With_Link_Names()
        {
            //// Arrange
            string webHookUrl = "https://hooks.slack.com/services/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";
            var data = new { text = "@oversea_brd 我是測試訊息 with link names", link_names = 1 };
            var json = JsonSerializer.Serialize(data);

            //// Act
            var target = this.GetTarget();
            target.SendAsync(webHookUrl, json);

            //// Assert
            Assert.True(true);
        }

        [Fact(Skip = "整合測試")]
        public void Test_Tag_Group_With_Subteam_Id()
        {
            //// Arrange
            string webHookUrl = "";
            var data = new { text = "<!subteam^SJGSP4PFW> 我是測試訊息 with subteam id" };
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