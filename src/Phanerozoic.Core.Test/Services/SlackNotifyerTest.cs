﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Services.Interfaces;
using Phanerozoic.Core.Services.Slacks;
using Xunit;

namespace Phanerozoic.Core.Test.Services
{
    public class SlackNotifyerTest
    {
        private readonly ISlackService _stubSlackService;
        private readonly IConfiguration _stubConfiguration;
        private readonly IServiceProvider _stubServiceProvider;

        public SlackNotifyerTest()
        {
            this._stubSlackService = Substitute.For<ISlackService>();
            this._stubConfiguration = Substitute.For<IConfiguration>();

            this._stubServiceProvider = Substitute.For<IServiceProvider>();
            this._stubServiceProvider.GetService<ISlackService>().Returns(this._stubSlackService);
            this._stubServiceProvider.GetService<IConfiguration>().Returns(this._stubConfiguration);
        }

        [Fact]
        public void Test涵蓋率未下降則不發通知()
        {
            //// Arrange
            var coverageEntity = new RepositoryCoverageEntity
            {
                Repository = "Phanerozoic"
            };

            var reportMethod = new CoverageEntity
            {
                Class = "AClass",
                Method = "AMethod",
                Coverage = 23,
            };
            var sheetMethod = new CoverageEntity
            {
                Class = "AClass",
                Method = "AMethod",
                Coverage = 23,
            };
            sheetMethod.UpdateCoverage(reportMethod);

            var methodList = new List<CoverageEntity>
            {
                sheetMethod,
            };

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(sheetMethod.ToString());
            var expectedMessage = stringBuilder.ToString();

            //// Act
            var target = GetTarget();
            target.Notify(coverageEntity, methodList);

            //// Assert
            this._stubSlackService.DidNotReceiveWithAnyArgs().SendAsync(string.Empty, Arg.Any<string>());
        }

        [Fact]
        public void Test涵蓋率下降的方法發出通知()
        {
            //// Arrange
            var coverageEntity = new RepositoryCoverageEntity
            {
                Repository = "Phanerozoic"
            };

            var method = new CoverageEntity
            {
                Class = "AClass",
                Method = "AMethod",
                Coverage = 23,
                TargetCoverage = 100,
            };
            var methodList = new List<CoverageEntity>
            {
                method,
            };

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(method.ToString());
            var expectedMessage = stringBuilder.ToString();

            var webHookUrl = "http://abc.com";
            this._stubConfiguration["Slack:WebHookUrl"].Returns(webHookUrl);

            //// Act
            var target = GetTarget();
            target.Notify(coverageEntity, methodList);

            //// Assert
            this._stubSlackService.Received(1).SendAsync(webHookUrl, Arg.Any<string>());
        }

        private SlackNotifyer GetTarget()
        {
            return new SlackNotifyer(this._stubServiceProvider);
        }
    }
}