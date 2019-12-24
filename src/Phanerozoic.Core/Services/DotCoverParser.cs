using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Phanerozoic.Core.Services
{
    public class DotCoverParser : IReportParser
    {
        private readonly IFileHelper _fileHelper;

        public DotCoverParser(IServiceProvider serviceProvider)
        {
            this._fileHelper = serviceProvider.GetRequiredService<IFileHelper>();
        }

        public List<CoverageEntity> Parser(ReportEntity reportEntity)
        {
            var json = this._fileHelper.ReadAllText(reportEntity.FilePath);
            var report = JsonSerializer.Deserialize<DotCoverReport>(json);

            var result = new List<CoverageEntity>();
            FindMethod(result, string.Empty, report.Children);

            return result;
        }

        /// <summary>
        /// 尋找 Kind = Method 的各階層的項目
        /// </summary>
        /// <param name="result">回傳值</param>
        /// <param name="source">目前的階層</param>
        private void FindMethod(List<CoverageEntity> result, string fullName, List<DotCoverReportChild> source)
        {
            if (source == null)
            {
                return;
            }

            foreach (var item in source)
            {
                if (item.Kind == Kind.Method)
                {
                    var covearge = new CoverageEntity
                    {
                        Class = fullName.Remove(fullName.Length - 1, 1),
                        Method = item.Name,
                        Coverage = (int)item.CoveragePercent,
                    };
                    result.Add(covearge);
                }
                else if (item.Kind == Kind.Assembly)
                {
                    FindMethod(result, fullName, item.Children);
                }
                else
                {
                    fullName += $"{item.Name}.";
                    FindMethod(result, fullName, item.Children);
                }
            }
        }
    }
}