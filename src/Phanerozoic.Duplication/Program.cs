using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services.Googles;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Duplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Console.WriteLine("* Duplicate");
            Duplicate(serviceProvider);

            serviceProvider.Dispose();
        }

        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IGoogleSheetsService, GoogleSheetsService>();
            serviceCollection.AddScoped<IDateTimeHelper, DateTimeHelper>();

            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("AppSettings.json.user", true, true)
                .Build();

            serviceCollection.AddSingleton<IConfiguration>(configurationRoot);
        }

        public static void Duplicate(ServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var googleSheetsService = serviceProvider.GetService<IGoogleSheetsService>();

            var sourceId = configuration["Google:Sheets:SourceId"];
            var targetId = configuration["Google:Sheets:TargetId"];

            Console.WriteLine($"Source Sheets ID: {sourceId}");
            Console.WriteLine($"Target Sheets ID: {targetId}");

            DuplicateCoverage(googleSheetsService, sourceId, targetId);
            DuplicateCoverageLog(serviceProvider, googleSheetsService, sourceId, targetId);
        }

        private static void DuplicateCoverage(IGoogleSheetsService googleSheetsService, string sourceId, string targetId)
        {
            var sheetName = "Coverage";

            //// Read
            var firstRow = 2;
            var maxRow = string.Empty;
            var startColumn = "A";
            var endColumn = "J";
            var range = $"{sheetName}!{startColumn}{firstRow}:{endColumn}{maxRow}";
            Console.WriteLine($"Source Range: {range}");
            var sourceList = googleSheetsService.GetValues(sourceId, range);
            var methodList = Program.SheetRangeToEntityList(sourceList);

            //// Write
            endColumn = "H";
            var targetRange = $"{sheetName}!{startColumn}{firstRow}:{endColumn}{sourceList.Count + 1}";
            Console.WriteLine($"Target Range: {targetRange}");
            var targetList = Program.EntityListToSheetRange(methodList);
            googleSheetsService.SetValue(targetId, targetRange, targetList);
        }

        private static void DuplicateCoverageLog(IServiceProvider serviceProvider, IGoogleSheetsService googleSheetsService, string sourceId, string targetId)
        {
            var dateTimeHelper = serviceProvider.GetService<IDateTimeHelper>();

            var now = dateTimeHelper.Now;
            var sheetName = now.ToString("yyyy");

            //// Read
            var firstRow = 1;
            var maxRow = string.Empty;
            var startColumn = "A";
            var endColumn = "BH";
            var range = $"{sheetName}!{startColumn}{firstRow}:{endColumn}{maxRow}";
            Console.WriteLine($"Source Range: {range}");
            var sourceList = googleSheetsService.GetValues(sourceId, range);

            //// Write
            var targetRange = $"{sheetName}!{startColumn}{firstRow}:{endColumn}{sourceList.Count + 1}";
            Console.WriteLine($"Target Range: {targetRange}");
            googleSheetsService.SetValue(targetId, targetRange, sourceList);
        }

        private static List<CoverageEntity> SheetRangeToEntityList(IList<IList<object>> sheetRange)
        {
            List<CoverageEntity> methodList = new List<CoverageEntity>();
            if (sheetRange != null && sheetRange.Count > 0)
            {
                foreach (var row in sheetRange)
                {
                    var methodEntity = new CoverageEntity
                    {
                        Repository = row[0].ToString().Trim(),
                        Project = row[1].ToString().Trim(),
                        Class = row[2].ToString().Trim(),
                        Method = row[3].ToString().Trim(),
                        Coverage = SheetHelper.ObjectToInt(row[4]),
                        Team = row[6].ToString().Trim(),
                        UpdatedDate = row.Count < 10 ? string.Empty : row[9].ToString().Trim(),
                        RawData = row,
                    };

                    if (string.IsNullOrWhiteSpace(methodEntity.Method) == false)
                    {
                        methodList.Add(methodEntity);
                    }
                }
            }

            return methodList;
        }

        private static IList<IList<object>> EntityListToSheetRange(List<CoverageEntity> methodList)
        {
            var rowList = new List<IList<object>>();
            foreach (var method in methodList)
            {
                var row = new List<object>
                {
                    method.Repository,
                    method.Project,
                    method.Class,
                    method.Method,
                    method.Coverage,
                    method.RawData[5],
                    method.Team,
                    method.UpdatedDate,
                };
                rowList.Add(row);
            }

            return rowList;
        }
    }
}