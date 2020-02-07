﻿using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;

namespace Phanerozoic.Core.Services
{
    public class CoverageProcessor : ICoverageProcessor
    {
        private readonly IFileHelper _fileHelper;
        private readonly IReportParser _reportParser;
        private readonly ICoverageUpdater _coverageUpdater;
        private readonly INotifyer _notifyer;
        private readonly ICoverageLogger _coverageLogger;

        public CoverageProcessor(IServiceProvider serviceProvider)
        {
            this._fileHelper = serviceProvider.GetRequiredService<IFileHelper>();
            this._reportParser = serviceProvider.GetRequiredService<IReportParser>();
            this._coverageUpdater = serviceProvider.GetRequiredService<ICoverageUpdater>();
            this._notifyer = serviceProvider.GetRequiredService<INotifyer>();
            this._coverageLogger = serviceProvider.GetRequiredService<ICoverageLogger>();
        }

        public void Process(ReportEntity reportEntity, CoverageEntity coverageEntity)
        {
            if (this._fileHelper.Exists(reportEntity.FilePath) == false)
            {
                Console.WriteLine($"File Not Found: {reportEntity.FilePath}");
                throw new FileNotFoundException("File Not Found!", reportEntity.FilePath);
            }

            //// Parser
            var methodList = this._reportParser.Parser(reportEntity);

            //// Update
            var updateMethodList = this._coverageUpdater.Update(coverageEntity, methodList);

            //// Notify
            this._notifyer.Notify(coverageEntity, updateMethodList);

            //// Log
            this._coverageLogger.Log(updateMethodList);
        }
    }
}