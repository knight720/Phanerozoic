using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services.Emails;
using Phanerozoic.Core.Services.Interfaces;
using Phanerozoic.Core.Services.Slacks;

namespace Phanerozoic.Core.Services
{
    public class CoverageProcessor : ICoverageProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileHelper _fileHelper;
        private readonly IReportParser _reportParser;
        private readonly ICoverageCollect _coverageCollect;
        private readonly ICoverageUpdater _coverageUpdater;
        private readonly INotifyer _slackNotifyer;
        private readonly INotifyer _emailNotifyer;
        private readonly ICoverageLogger _coverageLogger;
        private readonly IConfiguration _configuration;

        public CoverageProcessor(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._fileHelper = serviceProvider.GetRequiredService<IFileHelper>();
            this._reportParser = serviceProvider.GetRequiredService<IReportParser>();
            this._coverageCollect = serviceProvider.GetRequiredService<ICoverageCollect>();
            this._coverageUpdater = serviceProvider.GetRequiredService<ICoverageUpdater>();
            this._coverageLogger = serviceProvider.GetRequiredService<ICoverageLogger>();
            this._configuration = serviceProvider.GetRequiredService<IConfiguration>();
        }

        public void Process(ReportEntity reportEntity, RepositoryCoverageEntity coverageEntity)
        {
            if (this._fileHelper.Exists(reportEntity.FilePath) == false)
            {
                Console.WriteLine($"File Not Found: {reportEntity.FilePath}");
                throw new FileNotFoundException("File Not Found!", reportEntity.FilePath);
            }

            //// Parser
            Console.WriteLine("* Parser");
            var methodList = this._reportParser.Parser(coverageEntity, reportEntity);

            //// Update
            Console.WriteLine("* Update");
            var updateMethodList = this._coverageUpdater.Update(coverageEntity, methodList);

            //// Notify
            Console.WriteLine("* Notify");
            if (IsSendSlack())
            {
                this.GetSlackNotifyer().Notify(coverageEntity, updateMethodList);
            }
            this.GetEmailNotifyer().Notify(coverageEntity, updateMethodList);

            //// Log
            Console.WriteLine("* Log");
            this._coverageLogger.Log(updateMethodList);
        }

        /// <summary>
        /// 讀取涵蓋率
        /// </summary>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="coverageEntity">The coverage entity.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessParserAndCollect(ReportEntity reportEntity, RepositoryCoverageEntity coverageEntity)
        {
            if (this._fileHelper.Exists(reportEntity.FilePath) == false)
            {
                Console.WriteLine($"File Not Found: {reportEntity.FilePath}");
                throw new FileNotFoundException("File Not Found!", reportEntity.FilePath);
            }

            //// Parser
            Console.WriteLine("* Parser");
            var methodList = this._reportParser.Parser(coverageEntity, reportEntity);

            //// Log
            Console.WriteLine("* Collect");
            this._coverageCollect.Collect(coverageEntity, methodList);
        }

        /// <summary>
        /// 更新涵蓋率及通知
        /// </summary>
        /// <param name="reportEntity">The report entity.</param>
        /// <param name="coverageEntity">The coverage entity.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ProcessUpdateAndNotify(RepositoryCoverageEntity coverageEntity)
        {
            //// Load Coverage From Collect
            Console.WriteLine("* Collect");
            var methodList = this._coverageCollect.LoadCollect(coverageEntity);

            //// Update
            Console.WriteLine("* Update");
            var updateMethodList = this._coverageUpdater.Update(coverageEntity, methodList);

            //// Notify
            Console.WriteLine("* Notify");
            if (IsSendSlack())
            {
                this.GetSlackNotifyer().Notify(coverageEntity, updateMethodList);
            }
            this.GetEmailNotifyer().Notify(coverageEntity, updateMethodList);

            //// Log
            Console.WriteLine("* Log");
            this._coverageLogger.Log(updateMethodList);
        }

        protected virtual INotifyer GetSlackNotifyer()
        {
            var notifyList = this._serviceProvider.GetServices<INotifyer>();
            return notifyList.FirstOrDefault(i => i is SlackNotifyer);
        }

        protected virtual INotifyer GetEmailNotifyer()
        {
            var notifyList = this._serviceProvider.GetServices<INotifyer>();
            return notifyList.FirstOrDefault(i => i is EmailNotifyer);
        }

        protected virtual bool IsSendSlack()
        {
            return this._configuration.GetValue<bool>("Slack", true);
        }
    }
}