using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services.Interface;

namespace Phanerozoic.Core.Services.Googles
{
    public class GoogleSheetsReader : ICoverageReader
    {
        private readonly IGoogleSheetsService _googleSheetsService;
        private string _sheetsId;

        public GoogleSheetsReader(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this._googleSheetsService = serviceProvider.GetService<IGoogleSheetsService>();

            this._sheetsId = configuration["Google:Sheets:Id"];
        }

        public IList<CoverageEntity> GetList()
        {
            var startIndex = 1;
            var maxRow = string.Empty;
            var sheetName = "Coverage";
            IList<CoverageEntity> sheetMethodList = new List<CoverageEntity>();
            IList<IList<object>> values = this._googleSheetsService.GetValues(this._sheetsId, $"{sheetName}!A{startIndex + 1}:I{maxRow}");

            var index = startIndex;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    index++;
                    var methodEntity = new CoverageEntity
                    {
                        Repository = row[0].ToString().Trim(),
                        Project = row[1].ToString().Trim(),
                        Class = row[2].ToString().Trim(),
                        Method = row[3].ToString().Trim(),
                        Coverage = row.Count > 4 ? SheetHelper.ObjectToInt(row[4]) : 0,
                        TargetCoverage = row.Count > 9 ? SheetHelper.ObjectToInt(row[9]) : 0,
                        RawIndex = index,
                        RawData = row,
                    };

                    if (string.IsNullOrWhiteSpace(methodEntity.Method) == false)
                    {
                        sheetMethodList.Add(methodEntity);
                    }
                }
            }

            return sheetMethodList;
        }
    }
}