using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;

namespace Phanerozoic.Core.Services
{
    public class FileUpdater : ICoverageUpdater
    {
        private IFileHelper _fileHelper;

        public FileUpdater(IServiceProvider serviceProvider)
        {
            this._fileHelper = serviceProvider.GetService<IFileHelper>();
        }

        public IList<CoverageEntity> Update(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList)
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

            this._fileHelper.WriteAllText(coverageEntity.FilePath, contents);

            return methodList;
        }
    }
}