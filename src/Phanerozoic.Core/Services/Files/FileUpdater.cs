﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Files
{
    public class FileUpdater : ICoverageUpdater
    {
        private IFileHelper _fileHelper;

        public FileUpdater(IServiceProvider serviceProvider)
        {
            _fileHelper = serviceProvider.GetService<IFileHelper>();
        }

        public IList<CoverageEntity> Update(RepositoryCoverageEntity coverageEntity, IList<CoverageEntity> methodList)
        {
            var stringBuilder = new StringBuilder();

            var filterMethodList = methodList.Where(i => i.Class.StartsWith("Xunit") == false).ToList();

            filterMethodList.ForEach(i =>
            {
                var className = i.Class;
                var method = i.Method;
                method = method.Substring(0, method.IndexOf('('));
                var row = $"\"{className}\",\"{method}\",{i.Coverage}";
                stringBuilder.AppendLine(row);
            });

            string contents = stringBuilder.ToString();

            _fileHelper.WriteAllText(coverageEntity.CoverageFile, contents);

            return methodList;
        }
    }
}