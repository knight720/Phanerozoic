using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Console.Constents;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services;
using Phanerozoic.Core.Services.Emails;
using Phanerozoic.Core.Services.Files;
using Phanerozoic.Core.Services.Googles;
using Phanerozoic.Core.Services.Interfaces;
using Phanerozoic.Core.Services.Slacks;

namespace Phanerozoic.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            var configuration = ConfigureServices(serviceCollection, args);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var reportEntity = new ReportEntity
            {
                FilePath = configuration[Arguments.Report]?.Trim(),
            };

            var file = new FileInfo(reportEntity.FilePath);
            var fileName = file.Name;
            fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
            var coverageEntity = new CoreMethodCoverageEntity
            {
                CoverageFileName = $"{fileName}.csv",
                OutputPath = file.DirectoryName,
                Repository = configuration[Arguments.Repository]?.Trim(),
                Project = configuration[Arguments.Project]?.Trim(),
            };

            var coverageProcessor = serviceProvider.GetService<ICoverageProcessor>();

            var mode = string.IsNullOrWhiteSpace(configuration[Arguments.Mode]) == false ? configuration[Arguments.Mode].ToEnum<ModeType>() : ModeType.Full;

            System.Console.WriteLine($"Mode: {mode}");
            switch (mode)
            {
                case ModeType.Parse:
                    System.Console.WriteLine("Parser And Collect");
                    coverageProcessor.ProcessParserAndCollect(reportEntity, coverageEntity);
                    break;

                case ModeType.Update:
                    System.Console.WriteLine("Load From Collect. Update And Nofity");
                    coverageProcessor.ProcessUpdateAndNotify(coverageEntity);
                    break;

                case ModeType.Full:
                default:
                    System.Console.WriteLine("Parser, Update And Notify");
                    coverageProcessor.Process(reportEntity, coverageEntity);
                    break;
            }

            serviceProvider.Dispose();
        }

        public static IConfiguration ConfigureServices(IServiceCollection serviceCollection, string[] args)
        {
            serviceCollection.AddScoped<ICoverageProcessor, CoverageProcessor>();
            serviceCollection.AddScoped<IFileHelper, FileHelper>();
            serviceCollection.AddScoped<IReportParser, DotCoverParser>();
            serviceCollection.AddScoped<ICoverageCollect, FileCollect>();
            serviceCollection.AddScoped<ICoverageUpdater, GoogleSheetsUpdater>();
            serviceCollection.AddScoped<ICoverageReader, GoogleSheetsReader>();
            serviceCollection.AddScoped<IGoogleSheetsService, GoogleSheetsService>();
            serviceCollection.AddScoped<INotifyer, SlackNotifyer>();
            serviceCollection.AddScoped<ISlackService, SlackService>();
            serviceCollection.AddScoped<ICoverageLogger, GoogleSheetsLogger>();
            serviceCollection.AddScoped<IDateTimeHelper, DateTimeHelper>();
            serviceCollection.AddScoped<INotifyer, EmailNotifyer>();
            serviceCollection.AddScoped<IEmailService, GmailService>();
            serviceCollection.AddHttpClient();

            var switchMappings = new Dictionary<string, string>()
            {
                { "-m", Arguments.Mode },
                { "-r", Arguments.Repository },
                { "-p", Arguments.Project },
                { "-s", Arguments.Slack },
            };

            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("AppSettings.json.user", true, true)
                .AddCommandLine(args, switchMappings)
                .Build();

            serviceCollection.AddSingleton<IConfiguration>(configurationRoot);
            serviceCollection.AddHttpClient();

            return configurationRoot;
        }
    }
}