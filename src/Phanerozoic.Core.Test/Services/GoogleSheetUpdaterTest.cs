using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services;
using Phanerozoic.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace Phanerozoic.Core.Test.Services
{
    public class GoogleSheetUpdaterTest
    {
        private readonly IFileHelper _stubFileHelper;
        private readonly IConfiguration _stubConfiguration;
        private readonly IGoogleSheetsService _stubGoogleSheetsService;
        private readonly ICoverageReader _stubCoverageReader;
        private readonly IServiceProvider _stubServiceProvider;

        public GoogleSheetUpdaterTest()
        {
            this._stubFileHelper = Substitute.For<IFileHelper>();
            this._stubConfiguration = Substitute.For<IConfiguration>();
            this._stubGoogleSheetsService = Substitute.For<IGoogleSheetsService>();
            this._stubCoverageReader = Substitute.For<ICoverageReader>();

            this._stubServiceProvider = Substitute.For<IServiceProvider>();
            this._stubServiceProvider.GetService<IFileHelper>().Returns(this._stubFileHelper);
            this._stubServiceProvider.GetService<IConfiguration>().Returns(this._stubConfiguration);
            this._stubServiceProvider.GetService<IGoogleSheetsService>().Returns(this._stubGoogleSheetsService);
            this._stubServiceProvider.GetService<ICoverageReader>().Returns(this._stubCoverageReader);
        }

        [Fact(Skip ="取得資料已移至ICoverageReader")]
        public void Test_取得目前的涵蓋率()
        {
            //// Arrange
            var coverageEntity = new CoreMethodCoverageEntity();
            var methodList = new List<CoverageEntity>();

            this._stubConfiguration["Google.Sheets.SheetsId"].Returns("target Id");

            //// Act
            var target = new GoogleSheetsUpdater(this._stubServiceProvider, this._stubConfiguration);
            target.Update(coverageEntity, methodList);

            //// Assert
            this._stubGoogleSheetsService.Received(1).GetValues(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}