using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services;
using Xunit;

namespace Phanerozoic.Core.Test.Services
{
    public class CoverageProcessorTest
    {
        private readonly IServiceProvider _stubServiceProvider;
        private readonly IFileHelper _stubFileHelper;
        private readonly IReportParser _stubReportParser;
        private readonly ICoverageUpdater _stubCoverageUpdater;
        private readonly INotifyer _stubNotifyer;
        private readonly INotifyer _stubEmailNotifyer;
        private readonly ICoverageLogger _stubCoverageLogger;

        public CoverageProcessorTest()
        {
            this._stubFileHelper = Substitute.For<IFileHelper>();
            this._stubReportParser = Substitute.For<IReportParser>();
            this._stubCoverageUpdater = Substitute.For<ICoverageUpdater>();
            this._stubNotifyer = Substitute.For<INotifyer>();
            this._stubEmailNotifyer = Substitute.For<INotifyer>();
            this._stubCoverageLogger = Substitute.For<ICoverageLogger>();

            this._stubServiceProvider = Substitute.For<IServiceProvider>();
            this._stubServiceProvider.GetService<IFileHelper>().Returns(this._stubFileHelper);
            this._stubServiceProvider.GetService<IReportParser>().Returns(this._stubReportParser);
            this._stubServiceProvider.GetService<ICoverageUpdater>().Returns(this._stubCoverageUpdater);
            //this._stubServiceProvider.GetService<INotifyer>().Returns(this._stubNotifyer);
            //this._stubServiceProvider.GetService<INotifyer>().Returns(this._stubEmailNotifyer);
            //this._stubServiceProvider.GetService<INotifyer>().Returns(new SlackNotifyer(this._stubServiceProvider));
            //this._stubServiceProvider.GetService<INotifyer>().Returns(new EmailNotifyer(this._stubServiceProvider));
            this._stubServiceProvider.GetService<IEnumerable<INotifyer>>().Returns(new List<INotifyer> { this._stubNotifyer, this._stubEmailNotifyer });
            this._stubServiceProvider.GetService<ICoverageLogger>().Returns(this._stubCoverageLogger);

            //this._stubServiceProvider.GetServices<INotifyer>();
            //this._stubServiceProvider.GetServices<INotifyer>().Returns(new List<INotifyer> { this._stubNotifyer, this._stubEmailNotifyer });
        }

        [Fact(DisplayName = "Happy Path")]
        public void Test_Process_Flow()
        {
            //// arrange
            this._stubFileHelper.Exists(Arg.Any<string>()).Returns(true);
            var reportEntity = new ReportEntity
            {
                FilePath = "report.json"
            };
            var coverageEntity = new CoreMethodCoverageEntity
            {
                FilePath = "coverage.csv"
            };

            var target = GetTarget();

            //// act
            target.Process(reportEntity, coverageEntity);

            //// assert
            this._stubReportParser.Received(1).Parser(Arg.Any<CoreMethodCoverageEntity>(), Arg.Any<ReportEntity>());
            this._stubCoverageUpdater.Received(1).Update(Arg.Any<CoreMethodCoverageEntity>(), Arg.Any<IList<CoverageEntity>>());
            this._stubNotifyer.Received(1).Notify(Arg.Any<CoreMethodCoverageEntity>(), Arg.Any<IList<CoverageEntity>>());
            this._stubEmailNotifyer.Received(1).Notify(Arg.Any<CoreMethodCoverageEntity>(), Arg.Any<IList<CoverageEntity>>());
            this._stubCoverageLogger.Received(1).Log(Arg.Any<IList<CoverageEntity>>());
        }

        [Fact(DisplayName = "檔案不存在")]
        public void Test_Process_Flow_FileNotFound()
        {
            //// arrange
            this._stubFileHelper.Exists(Arg.Any<string>()).Returns(false);
            var reportEntity = new ReportEntity
            {
                FilePath = "report.json"
            };
            var coverageEntity = new CoreMethodCoverageEntity
            {
                FilePath = "coverage.csv"
            };

            var target = GetTarget();

            //// act
            Action action = () => target.Process(reportEntity, coverageEntity);

            //// assert
            action.Should()
                  .Throw<FileNotFoundException>()
                  .WithMessage("File Not Found!");
        }

        private CoverageProcessor GetTarget()
        {
            var target = new StubCoverageProcessor(this._stubServiceProvider);
            target.StubSlackNotifyer = this._stubNotifyer;
            target.StubEmailNotifyer = this._stubEmailNotifyer;
            return target;
        }
    }
}