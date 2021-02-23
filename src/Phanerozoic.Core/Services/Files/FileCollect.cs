using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Files
{
    public class FileCollect : ICoverageCollect
    {
        public static string Format = "yyyyMMdd";
        private readonly IDateTimeHelper _dateTimeHelper;

        public FileCollect(IDateTimeHelper dateTimeHelper)
        {
            this._dateTimeHelper = dateTimeHelper;
        }

        /// <summary>
        /// 將同一個 Solution 不同 Test Project 的 Coverage 合併
        /// </summary>
        /// <param name="coverageEntity">The coverage entity.</param>
        /// <param name="methodList">The method list.</param>
        /// <returns></returns>
        public string Collect(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList)
        {
            var fileMethodList = LoadCollect(coverageEntity);

            //// Update Coverage
            MergeCoverage(methodList, ref fileMethodList);

            var fileName = GetFileName(coverageEntity);

            //// Save File
            SaveCoverage(fileName, fileMethodList);

            return fileName;
        }

        /// <summary>
        /// Loads the collect.
        /// </summary>
        /// <param name="coverageEntity">The coverage entity.</param>
        /// <returns></returns>
        public IList<CoverageEntity> LoadCollect(CoreMethodCoverageEntity coverageEntity)
        {
            var fileName = GetFileName(coverageEntity);

            Console.WriteLine($"Collect File Name: {fileName}");

            //// Load File
            var fileMethodList = LoadCoverage(fileName);

            return fileMethodList;
        }

        /// <summary>
        /// Loads the coverage.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private IList<CoverageEntity> LoadCoverage(string fileName)
        {
            IList<CoverageEntity> methodList = new List<CoverageEntity>();
            if (File.Exists(fileName))
            {
                var data = File.ReadAllText(fileName);
                methodList = JsonSerializer.Deserialize<IList<CoverageEntity>>(data);
            }
            return methodList;
        }

        /// <summary>
        /// Merges the coverage.
        /// </summary>
        /// <param name="methodList">The method list.</param>
        /// <param name="fileMethodList">The file method list.</param>
        internal void MergeCoverage(IList<CoverageEntity> methodList, ref IList<CoverageEntity> fileMethodList)
        {
            var newMethodList = new List<CoverageEntity>();
            var updateCount = 0;
            var addCount = 0;
            foreach (var source in methodList)
            {
                var method = fileMethodList.FirstOrDefault(i => i.Equals(source));
                if (method != null)
                {
                    if (source.Coverage > method.Coverage)
                    {
                        Console.WriteLine($"{method.ToString()} to {source.Coverage}");
                        updateCount++;
                        method.Coverage = source.Coverage;
                    }
                }
                else
                {
                    addCount++;
                    newMethodList.Add(source);
                }
            }
            Console.WriteLine($"FileCollect - Total:{methodList.Count}, Update:{updateCount}, Add:{addCount}");

            fileMethodList = fileMethodList.Concat(newMethodList).ToList();
        }

        /// <summary>
        /// Saves the coverage.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileMethodList">The file method list.</param>
        protected virtual void SaveCoverage(string fileName, IList<CoverageEntity> fileMethodList)
        {
            var data = JsonSerializer.Serialize(fileMethodList);
            File.WriteAllText(fileName, data);
        }

        private string GetFileName(CoreMethodCoverageEntity coreMethodCoverage)
        {
            return Path.Combine(coreMethodCoverage.OutputPath, $"{coreMethodCoverage.Repository}_{this._dateTimeHelper.Now.ToString(Format)}.json");
        }
    }
}