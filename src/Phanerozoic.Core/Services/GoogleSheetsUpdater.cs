using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services
{
    public class GoogleSheetsUpdater : ICoverageUpdater
    {
        private readonly IConfiguration _configuration;
        private readonly IGoogleSheetsService _googleSheetsService;
        private readonly ICoverageReader _coverageReader;
        private string _sheetsId;
        private int _interval;

        private static Dictionary<CoverageStatus, string> SymbolDictionary;

        static GoogleSheetsUpdater()
        {
            SymbolDictionary = new Dictionary<CoverageStatus, string>
            {
                { CoverageStatus.Unchange, "=" },
                { CoverageStatus.Up, "▲" },
                { CoverageStatus.Down, "▼" }
            };
        }

        public GoogleSheetsUpdater(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._googleSheetsService = serviceProvider.GetService<IGoogleSheetsService>();
            this._coverageReader = serviceProvider.GetService<ICoverageReader>();

            this._sheetsId = this._configuration["Google:Sheets:Id"];
            this._interval = int.Parse(this._configuration["Google:Sheets:Interval"]);

            Console.WriteLine($"Target Sheets ID: {this._sheetsId}");
        }

        public IList<CoverageEntity> Update(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> reportMethodList)
        {
            var reportMethodTotalCount = reportMethodList.Count;
            reportMethodList = this.FilterMethod(coverageEntity, reportMethodList);
            Console.WriteLine("** Report Method");
            Console.WriteLine($"Repository: {coverageEntity.Repository}, Project: {coverageEntity.Project}, Method Count: {reportMethodList.Count}/{reportMethodTotalCount}");

            IList<CoverageEntity> sheetMethodList = this._coverageReader.GetList();

            var sheetMethodTotalCount = sheetMethodList.Count;
            sheetMethodList = this.FilterMethod(coverageEntity, sheetMethodList);
            Console.WriteLine("** Sheet Method");
            Console.WriteLine($"Repository: {coverageEntity.Repository}, Project: {coverageEntity.Project}, Method Count: {sheetMethodList.Count}/{sheetMethodTotalCount}");
            if (sheetMethodList.Count <= 0)
            {
                return sheetMethodList;
            }

            var updateCount = 0;
            foreach (var coreMethod in sheetMethodList)
            {
                var reportMethod = reportMethodList.FirstOrDefault(i => i.Equals(coreMethod));

                if (reportMethod == null)
                {
                    continue;
                }
                updateCount++;

                coreMethod.UpdateCoverage(reportMethod);

                var symbol = SymbolDictionary[coreMethod.Status];

                Console.WriteLine($"{coreMethod.Class}.{coreMethod.Method}: {coreMethod.LastCoverage} {symbol} {coreMethod.Coverage}, Target: {coreMethod.TargetCoverage}, {(coreMethod.IsPass ? "Pass" : "Fail")}");

                if (coreMethod.Status != CoverageStatus.Unchange || coreMethod.Coverage == 0)
                {
                    this.UpdateCell($"F{coreMethod.RawIndex}", coreMethod.Coverage);
                }
                //// 目標涵蓋率小於0則不更新
                if (coreMethod.TargetCoverage > -1 && (coreMethod.TargetCoverage != coreMethod.NewTargetCoverage || coreMethod.NewTargetCoverage == 0))
                {
                    this.UpdateCell($"H{coreMethod.RawIndex}", coreMethod.NewTargetCoverage);
                }
                this.UpdateCell($"L{coreMethod.RawIndex}", DateTime.Now.ToString(DateTimeHelper.Format));
            }
            Console.WriteLine($"Update Rate: {updateCount}/{sheetMethodList.Count}");

            return sheetMethodList;
        }

        /// <summary>
        /// 只保留指定的 Repository 與 Project 下的方法
        /// </summary>
        /// <param name="coverageEntity"></param>
        /// <param name="methodList"></param>
        /// <returns></returns>
        private IList<CoverageEntity> FilterMethod(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList)
        {
            return methodList.Where(i =>
                            i.Repository == coverageEntity.Repository &&
                            i.Project == coverageEntity.Project
                            ).ToList();
        }

        private void UpdateCell(string range, object value)
        {
            var updateValues = new List<IList<object>>
                {
                    new List<object>
                    {
                        value
                    }
                };

            this._googleSheetsService.SetValue(this._sheetsId, range, updateValues);
            Thread.Sleep(this._interval);
        }
    }
}