using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services.Googles;
using Phanerozoic.Core.Services.Interfaces;
using Xunit;

namespace Phanerozoic.Core.Services.Tests
{
    public class CoverageLogTests
    {
        private readonly IGoogleSheetsService _stubGoogleSheetsService;
        private readonly IDateTimeHelper _stubDateTimeHelper;
        private readonly IServiceProvider _stubServiceProvider;
        private readonly IConfiguration _stubConfiguration;

        public CoverageLogTests()
        {
            this._stubGoogleSheetsService = Substitute.For<IGoogleSheetsService>();
            this._stubConfiguration = Substitute.For<IConfiguration>();
            this._stubDateTimeHelper = Substitute.For<IDateTimeHelper>();

            this._stubServiceProvider = Substitute.For<IServiceProvider>();
            this._stubServiceProvider.GetService<IGoogleSheetsService>().Returns(this._stubGoogleSheetsService);
            this._stubServiceProvider.GetService<IDateTimeHelper>().Returns(this._stubDateTimeHelper);
        }

        [Fact(DisplayName = "每年一 Sheet,每周一 Column")]
        public void Test_Coverage_Log_Write_Cell_Column()
        {
            this.SheetRangeAssert(new DateTime(2019, 1, 1), "2019!F1");
            this.SheetRangeAssert(new DateTime(2019, 1, 5), "2019!F1");
            this.SheetRangeAssert(new DateTime(2019, 1, 6), "2019!G1");
            this.SheetRangeAssert(new DateTime(2019, 12, 28), "2019!BE1");
            this.SheetRangeAssert(new DateTime(2019, 12, 29), "2019!BF1");
            this.SheetRangeAssert(new DateTime(2019, 12, 31), "2019!BF1");
            this.SheetRangeAssert(new DateTime(2020, 1, 1), "2020!F1");
            this.SheetRangeAssert(new DateTime(2020, 1, 4), "2020!F1");
            this.SheetRangeAssert(new DateTime(2020, 1, 5), "2020!G1");
        }

        private GoogleSheetsLogger GetTarget()
        {
            return new GoogleSheetsLogger(this._stubServiceProvider, this._stubConfiguration);
        }

        private void SheetRangeAssert(DateTime time, string range)
        {
            //// Initial
            this._stubGoogleSheetsService.ClearReceivedCalls();

            //// Arrange
            var methodList = new List<CoverageEntity>
            {
                new CoverageEntity()
            };
            this._stubDateTimeHelper.Now.Returns(time);
            this._stubConfiguration["Google:Sheets:Interval"].Returns("1000");

            //// Act
            GetTarget().Log(methodList);

            //// Assert
            this._stubGoogleSheetsService.Received(1).SetValue(Arg.Any<string>(), range, Arg.Any<IList<IList<object>>>());
        }
    }
}