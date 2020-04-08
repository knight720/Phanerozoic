using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace Phanerozoic.Core.Test.Services
{
    public class FileUpdaterTests
    {
        private IServiceProvider _stubServiceProvider;
        private IFileHelper _stubFileHelper;

        public FileUpdaterTests()
        {
            this._stubFileHelper = Substitute.For<IFileHelper>();

            this._stubServiceProvider = Substitute.For<IServiceProvider>();
            this._stubServiceProvider.GetService<IFileHelper>().Returns(this._stubFileHelper);
        }

        [Fact]
        public void Test_Update_Flow()
        {
            //// Arrange
            var methodList = new List<CoverageEntity>();
            var coverageEntity = new CoreMethodCoverageEntity
            {
                FilePath = "a.txt"
            };

            //// Act
            var target = new FileUpdater(this._stubServiceProvider);
            target.Update(coverageEntity, methodList);

            //// Assert
            this._stubFileHelper.Received(1).WriteAllText(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}