using System;
using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Xunit;

namespace Phanerozoic.Core.Services.Tests
{
    public class DotCoverParserTests
    {
        private readonly IServiceProvider _stubServiceProvider;
        private readonly IFileHelper _stubFileHelper;
        private readonly IConfiguration _stubConfiguration;

        public DotCoverParserTests()
        {
            this._stubFileHelper = Substitute.For<IFileHelper>();
            this._stubConfiguration = Substitute.For<IConfiguration>();

            this._stubServiceProvider = Substitute.For<IServiceProvider>();
            this._stubServiceProvider.GetService<IFileHelper>().Returns(this._stubFileHelper);
        }

        [Fact(DisplayName = "取得涵蓋率資料")]
        public void ParserTest()
        {
            //// arrange
            var report = new DotCoverReport
            {
                DotCoverVersion = "2019.2.3",
                Kind = Kind.Root,
                CoveredStatements = 11937,
                TotalStatements = 160364,
                CoveragePercent = 7,
                Children = new List<DotCoverReportChild>
                {
                    new DotCoverReportChild
                    {
                        Kind = Kind.Assembly,
                        Name = "Phanerozoic.Core",
                        CoveredStatements = 11937,
                        TotalStatements = 160364,
                        CoveragePercent = 7,
                        Children = new List<DotCoverReportChild>
                        {
                            new DotCoverReportChild
                            {
                                Kind = Kind.Assembly,
                                Name = "Phanerozoic.Core",
                                CoveredStatements = 11937,
                                TotalStatements = 160364,
                                CoveragePercent = 7,
                                Children = new List<DotCoverReportChild>
                                {
                                    new DotCoverReportChild
                                    {
                                        Kind = Kind.Namespace,
                                        Name = "Phanerozoic.Core.Services",
                                        CoveredStatements = 22,
                                        TotalStatements = 11,
                                        CoveragePercent = 7,
                                        Children = new List<DotCoverReportChild>
                                        {
                                            new DotCoverReportChild
                                            {
                                                Kind = Kind.Type,
                                                Name = "DotCoverParser",
                                                CoveredStatements = 22,
                                                TotalStatements = 11,
                                                CoveragePercent = 7,
                                                Children = new List<DotCoverReportChild>
                                                {
                                                    new DotCoverReportChild
                                                    {
                                                        Kind = Kind.Method,
                                                        Name = "Parser(ReportEntity):List<CoverageEntity>",
                                                        CoveredStatements = 22,
                                                        TotalStatements = 11,
                                                        CoveragePercent = 7,
                                                        Children = new List<DotCoverReportChild>
                                                        {
                                                        }
                                                    }
                                                }
                                            },
                                        }
                                    },
                                }
                            },
                        }
                    },
                }
            };
            string reportJson = JsonSerializer.Serialize(report);

            this._stubFileHelper.ReadAllText(Arg.Any<string>()).Returns(reportJson);

            var coverageEntity = new CoverageEntity
            {
                Repository = "Phanerozoic",
            };

            var reportEntity = new ReportEntity
            {
                FilePath = "report.json",
            };

            //// act
            var target = GetTarget();
            var actual = target.Parser(coverageEntity, reportEntity);

            actual.Count.Should().Be(3);
        }

        private DotCoverParser GetTarget()
        {
            return new DotCoverParser(this._stubServiceProvider, this._stubConfiguration);
        }
    }
}