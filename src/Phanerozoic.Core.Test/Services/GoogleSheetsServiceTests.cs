using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace Phanerozoic.Core.Services.Tests
{
    public class GoogleSheetsServiceTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public GoogleSheetsServiceTests()
        {
            this._configuration = Substitute.For<IConfiguration>();

            this._serviceProvider = Substitute.For<IServiceProvider>();
            this._serviceProvider.GetService<IConfiguration>().Returns(this._configuration);
        }

        [Fact(DisplayName = "整合測試", Skip = "開發時使用")]
        public void CreateSheetTest()
        {
            //// Arrange
            string sheetId = "1mzR8FODXLPj0-i-2UspDWpu7xqpFfBFI_mOfvb-gScU";

            string sheetName = "Test";

            this._configuration["Google:Credential:Type"].Returns("User");
            this._configuration["Google:Credential:File"].Returns("credentials.json");

            //// Act
            var target = this.GetTarget();
            target.CreateSheet(sheetId, sheetName);

            //// Assert
            Assert.True(true);
        }

        private GoogleSheetsService GetTarget()
        {
            return new GoogleSheetsService(this._serviceProvider);
        }
    }
}