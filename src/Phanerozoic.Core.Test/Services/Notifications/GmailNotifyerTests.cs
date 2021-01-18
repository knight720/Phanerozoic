using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Services.Interfaces;
using Xunit;

namespace Phanerozoic.Core.Services.Notifications.Tests
{
    public class GmailNotifyerTests
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IServiceProvider _serviceProvider;
        public GmailNotifyerTests()
        {
            this._configuration = Substitute.For<IConfiguration>();
            this._emailService = Substitute.For<IEmailService>();

            this._serviceProvider = Substitute.For<IServiceProvider>();
            this._serviceProvider.GetService<IConfiguration>().Returns(this._configuration);
            this._serviceProvider.GetService<IEmailService>().Returns(this._emailService);
        }
        [Fact()]
        public void NotifyTest()
        {
            //// Assert
            string repository = "Phanerozoic";
            string project = "Phanerozoic.Core";
            var coverageEntity = new CoreMethodCoverageEntity
            {
                Repository = repository,
                Project = project,
            };
            var methodList = new List<CoverageEntity>
            {
                new CoverageEntity
                {
                    Repository = repository,
                    Project = project,
                    Class = "SomeClass",
                    Method = "SomeMethod",
                    Coverage = 12,
                }
            };
            this._configuration["Notification:To"].Returns("user@mail.com");

            //// Act
            EmailNotifyer target = this.GetTarget();
            target.Notify(coverageEntity, methodList);

            //// Arrange

        }

        private EmailNotifyer GetTarget()
        {
            return new EmailNotifyer(this._serviceProvider);
        }
    }
}