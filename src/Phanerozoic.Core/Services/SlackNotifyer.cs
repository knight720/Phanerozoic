using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;

namespace Phanerozoic.Core.Services
{
    public class SlackNotifyer : INotifyer
    {
        private readonly ISlackService _slackService;

        private string _webHookUrl;

        private IDictionary<string, string> _slackGroupIdDictionary;

        public SlackNotifyer(IServiceProvider serviceProvider)
        {
            this._slackService = serviceProvider.GetService<ISlackService>();
            var configuration = serviceProvider.GetService<IConfiguration>();

            this._webHookUrl = configuration["Slack:WebHookUrl"];

            this._slackGroupIdDictionary = configuration.GetSection("Slack:GroupId").Get<Dictionary<string, string>>();
        }

        public void Notify(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList)
        {
            var slackMessageJson = this.GetSlackMessage(coverageEntity, methodList);

            if (string.IsNullOrWhiteSpace(slackMessageJson))
            {
                return;
            }

            this._slackService.SendAsync(this._webHookUrl, slackMessageJson);
        }

        private string GetMessage(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Phanerozoic Notify @{DateTime.Now.ToString(DateTimeHelper.Format)}");

            var failCount = methodList.Count(i => i.IsPass == false);

            stringBuilder.AppendLine($"> Repository: {coverageEntity.Repository}, 涵蓋率未通過數量 {failCount}");

            foreach (var method in methodList)
            {
                if (method.IsPass == false)
                {
                    var msg = $"{method.Class}.{method.Method}: {method.Coverage} < {method.TargetCoverage}";
                    stringBuilder.AppendLine(msg);
                }
            }

            var data = new { text = stringBuilder.ToString() };
            var json = JsonSerializer.Serialize(data);

            return json;
        }

        private string GetSlackMessage(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList)
        {
            var failCount = methodList.Count(i => i.IsPass == false);
            if (failCount == 0)
            {
                return null;
            }

            var color = failCount > 0 ? "#FF0000" : "#00FF00";
            var attachment = new Attachment
            {
                Color = color,
                AuthorName = $"Repository: {coverageEntity.Repository}",
                Title = $"Project: {coverageEntity.Project} 涵蓋率未通過數量: {failCount}",
                Footer = $"Phanerozoic Notifyer",
            };

            var stringBuilder = new StringBuilder();
            foreach (var method in methodList)
            {
                if (method.IsPass == false)
                {
                    var msg = $"{method.ToString()} < {method.TargetCoverage}";
                    msg += this.TagGroup(method);
                    stringBuilder.AppendLine(msg);
                }
            }
            attachment.Text = stringBuilder.ToString();

            var slackMessage = new SlackMessage
            {
                Attachments = new List<Attachment>{
                    attachment
                }
            };

            return slackMessage.ToJson();
        }

        private string TagGroup(CoverageEntity method)
        {
            var message = string.Empty;

            if (string.IsNullOrEmpty(method.Team))
            {
                return message;
            }

            var teamArray = method.Team.Split(',');
            var groupList = new List<string>();
            foreach (var team in teamArray)
            {
                var teamName = team.ToLower();
                var groupId = this._slackGroupIdDictionary.ContainsKey(teamName) ? $"<!subteam^{this._slackGroupIdDictionary[teamName]}>" : $"`{team}`";
                groupList.Add(groupId);
            }

            if (groupList.Count > 0)
            {
                message = $", {string.Join(" ", groupList)}";
            }

            return message;
        }
    }
}