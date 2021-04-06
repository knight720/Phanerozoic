using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Slacks
{
    public class SlackNotifyer : INotifyer
    {
        private readonly ISlackService _slackService;

        private readonly string _webHookUrl;

        private readonly IDictionary<string, string> _slackGroupIdDictionary;

        private readonly IDictionary<MethodLevel, string> _levelColorDictionary;

        public SlackNotifyer(IServiceProvider serviceProvider)
        {
            _slackService = serviceProvider.GetService<ISlackService>();
            var configuration = serviceProvider.GetService<IConfiguration>();

            _webHookUrl = configuration["Slack:WebHookUrl"];

            _slackGroupIdDictionary = configuration.GetSection("Slack:GroupId").Get<Dictionary<string, string>>();

            this._levelColorDictionary = new Dictionary<MethodLevel, string>
            {
                { MethodLevel.High, "#FF0000"} ,
                { MethodLevel.Middle, "#FFA500"} ,
            };
        }

        public void Notify(RepositoryCoverageEntity coverageEntity, IList<CoverageEntity> methodList)
        {
            foreach (var level in this._levelColorDictionary.Keys)
            {
                var levelMethodList = methodList.Where(i => i.Level == level).ToList();

                var slackMessageJson = GetSlackMessage(coverageEntity, levelMethodList, level);

                if (string.IsNullOrWhiteSpace(slackMessageJson))
                {
                    return;
                }

                _slackService.SendAsync(_webHookUrl, slackMessageJson);
            }
        }

        private string GetMessage(RepositoryCoverageEntity coverageEntity, IList<CoverageEntity> methodList)
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

        private string GetSlackMessage(RepositoryCoverageEntity coverageEntity, IList<CoverageEntity> methodList, MethodLevel level)
        {
            var failCount = methodList.Count(i => i.IsPass == false);
            if (failCount == 0)
            {
                return null;
            }

            var color = this._levelColorDictionary[level];
            color = failCount > 0 ? color : "#00FF00";

            var project = string.Empty;
            if (string.IsNullOrWhiteSpace(coverageEntity.Project) == false)
            {
                project = $"Project: {coverageEntity.Project}, ";
            }
            var title = $"{project}{level} 涵蓋率未通過數量: {failCount}";
            var attachment = new Attachment
            {
                Color = color,
                AuthorName = $"Repository: {coverageEntity.Repository}",
                Title = title,
                Footer = $"Phanerozoic Notifyer",
            };

            var stringBuilder = new StringBuilder();
            foreach (var method in methodList)
            {
                if (method.IsPass == false)
                {
                    var msg = $"{method.ToString()} < {method.TargetCoverage}";
                    msg += TagGroup(method);
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
                var groupId = _slackGroupIdDictionary.ContainsKey(teamName) ? $"<!subteam^{_slackGroupIdDictionary[teamName]}>" : $"`{team}`";
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