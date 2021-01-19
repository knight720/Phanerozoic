using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Googles
{
    public class GoogleSheetsLogger : ICoverageLogger
    {
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGoogleSheetsService _googleSheetsService;
        private readonly IConfiguration _configuration;
        private string _sheetsId;

        public GoogleSheetsLogger(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _configuration = configuration;
            _dateTimeHelper = serviceProvider.GetService<IDateTimeHelper>();
            _googleSheetsService = serviceProvider.GetService<IGoogleSheetsService>();

            _sheetsId = _configuration["Google:Sheets:Id"];
        }

        public void Log(IList<CoverageEntity> methodList)
        {
            //// Load Sheet Log Data
            var currentMethodList = GetCurrentMethodList();

            //// Sync Method and Coverage
            var newMethodList = new List<CoverageEntity>();
            foreach (var method in methodList)
            {
                var methodLog = currentMethodList.FirstOrDefault(i => i.Equals(method));

                if (methodLog != null)
                {
                    methodLog.UpdateCoverage(method);
                }
                else
                {
                    newMethodList.Add(method);
                }
            }

            //// Write Log Data
            int firstColumn = 5;
            var now = _dateTimeHelper.Now;
            var col = GetColumnLetterByWeek(firstColumn, now);

            //// Write Method
            Console.WriteLine("** Write Coverage Log");
            foreach (var method in currentMethodList)
            {
                if (method.Status != CoverageStatus.Unchange)
                {
                    Console.WriteLine($"{method.ToString()}");
                    var range = $"{now.Year}!{col.columnLetter}{method.RawIndex}";
                    var values = SheetHelper.ObjectToValues(method.Coverage);
                    _googleSheetsService.SetValue(_sheetsId, range, values);
                }
            }

            //// Write New Method
            Console.WriteLine("** Write New Method");
            var index = currentMethodList.Count + 1;
            foreach (var method in newMethodList)
            {
                Console.WriteLine($"{method.ToString()}");

                ++index;
                var range = $"{now.Year}!A{index}:{col.columnLetter}{index}";

                var row = new object[col.column];
                row[0] = method.Repository;
                row[1] = method.Project;
                row[2] = method.Namespace;
                row[3] = method.Class;
                row[4] = method.Method;
                row[col.column - 1] = method.Coverage;
                var values = new List<IList<object>> { row };

                _googleSheetsService.SetValue(_sheetsId, range, values);
            }
        }

        private (int column, string columnLetter) GetColumnLetterByWeek(int firstColumn, DateTime now)
        {
            var week = GetWeek(now);
            var column = firstColumn + week;
            var columnLetter = SheetHelper.ColumnToLetter(column);

            //// Write Column Name
            var columnName = $"{week}({now.ToString("MM/dd")})";
            Console.WriteLine($"Write Column: {columnName}");
            var range = $"{now.Year}!{columnLetter}1";
            var values = SheetHelper.ObjectToValues(columnName);
            _googleSheetsService.SetValue(_sheetsId, range, values);

            return (column, columnLetter);
        }

        private (int column, string columnLetter) GetColumnLetterByDay(int firstColumn, DateTime now)
        {
            var dayOfYear = now.DayOfYear;
            var column = firstColumn + dayOfYear;
            var columnLetter = SheetHelper.ColumnToLetter(column);

            //// Write Column Name
            var columnName = $"{dayOfYear}({now.ToString("MM/dd")})";
            Console.WriteLine($"Write Column: {columnName}");
            var range = $"{now.Year}!{columnLetter}1";
            var values = SheetHelper.ObjectToValues(columnName);
            _googleSheetsService.SetValue(_sheetsId, range, values);

            return (column, columnLetter);
        }

        private int GetWeek(DateTime now)
        {
            var week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            return week;
        }

        private List<CoverageEntity> GetCurrentMethodList()
        {
            var now = _dateTimeHelper.Now;
            var startIndex = 1;
            var maxRow = string.Empty;
            List<CoverageEntity> methodLogList = new List<CoverageEntity>();
            IList<IList<object>> values = _googleSheetsService.GetValues(_sheetsId, $"{now.Year}!A{startIndex + 1}:I{maxRow}");

            var index = startIndex;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    if (row.Count < 5)
                    {
                        continue;
                    }

                    index++;
                    var methodEntity = new CoverageEntity
                    {
                        Repository = row[0].ToString().Trim(),
                        Project = row[1].ToString().Trim(),
                        Namespace = row[2].ToString().Trim(),
                        Class = row[3].ToString().Trim(),
                        Method = row[4].ToString().Trim(),
                        RawIndex = index,
                        RawData = row,
                    };

                    if (string.IsNullOrWhiteSpace(methodEntity.Method) == false)
                    {
                        methodLogList.Add(methodEntity);
                    }
                }
            }
            return methodLogList;
        }
    }
}