using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Xunit;

namespace Phanerozoic.Core.Services.Files.Tests
{
    public class FileCollectTests
    {
        private readonly IDateTimeHelper _stubDateTimeHelper;

        public FileCollectTests()
        {
            this._stubDateTimeHelper = Substitute.For<IDateTimeHelper>();
        }

        [Fact(DisplayName = "檔名應為 Project + 年月日")]
        public void Collect_FileName()
        {
            //// Arrange
            var projectName = "Office.Project";
            var dateTime = new DateTime(2020, 1, 18);
            var excpted = $"{projectName}_{dateTime.ToString("yyyyMMdd")}.json";

            IList<CoverageEntity> methodList = new List<CoverageEntity>();
            CoreMethodCoverageEntity coverageEntity = new CoreMethodCoverageEntity
            {
                Project = projectName,
            };

            this._stubDateTimeHelper.Now.Returns(dateTime);

            //// Act
            var actual = GetTarget().Collect(coverageEntity, methodList);

            //// Assert
            actual.Should().BeEquivalentTo(excpted);
        }

        [Fact(DisplayName = "不更新涵蓋率10小於20")]
        public void MergeCoverage_NoUdpate()
        {
            //// Arrange
            var project = "Phanerozoic.Core";
            var namespaceName = "Phanerozoic.Core.Services.Files";
            var className = "FileCollect";
            IList<CoverageEntity> methodList = new List<CoverageEntity>
            {
                new CoverageEntity
                {
                    Project = project,
                    Namespace = namespaceName,
                    Class = className,
                    Method = "A",
                    Coverage = 10,
                }
            };

            IList<CoverageEntity> fileMethodList = new List<CoverageEntity>
            {
                new CoverageEntity
                {
                    Project = project,
                    Namespace = namespaceName,
                    Class = className,
                    Method = "A",
                    Coverage = 20,
                }
            };

            //// Act
            GetTarget().MergeCoverage(methodList, ref fileMethodList);

            //// Assert
            fileMethodList.Count.Should().Be(1);
            fileMethodList[0].Coverage.Should().Be(20);
        }

        [Fact(DisplayName = "更新涵蓋率30大於20")]
        public void MergeCoverage_Udpate()
        {
            //// Arrange
            var project = "Phanerozoic.Core";
            var namespaceName = "Phanerozoic.Core.Services.Files";
            var className = "FileCollect";
            IList<CoverageEntity> methodList = new List<CoverageEntity>
            {
                new CoverageEntity
                {
                    Project = project,
                    Namespace = namespaceName,
                    Class = className,
                    Method = "A",
                    Coverage = 30,
                }
            };

            IList<CoverageEntity> fileMethodList = new List<CoverageEntity>
            {
                new CoverageEntity
                {
                    Project = project,
                    Namespace = namespaceName,
                    Class = className,
                    Method = "A",
                    Coverage = 20,
                }
            };

            //// Act
            GetTarget().MergeCoverage(methodList, ref fileMethodList);

            //// Assert
            fileMethodList.Count.Should().Be(1);
            fileMethodList[0].Coverage.Should().Be(30);
        }

        [Fact(DisplayName = "新增方法到清單")]
        public void MergeCoverage_Add()
        {
            //// Arrange
            var project = "Phanerozoic.Core";
            var namespaceName = "Phanerozoic.Core.Services.Files";
            var className = "FileCollect";
            IList<CoverageEntity> methodList = new List<CoverageEntity>
            {
                new CoverageEntity
                {
                    Project = project,
                    Namespace = namespaceName,
                    Class = className,
                    Method = "A",
                    Coverage = 10,
                }
            };

            IList<CoverageEntity> fileMethodList = new List<CoverageEntity>
            {
                new CoverageEntity
                {
                    Project = project,
                    Namespace = namespaceName,
                    Class = className,
                    Method = "B",
                    Coverage = 10,
                }
            };

            //// Act
            GetTarget().MergeCoverage(methodList, ref fileMethodList);

            //// Assert
            fileMethodList.Count.Should().Be(2);
        }

        private FileCollect GetTarget()
        {
            return new FileCollect(this._stubDateTimeHelper);
        }
    }
}