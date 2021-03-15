using System.IO;

namespace Phanerozoic.Core.Entities
{
    public class RepositoryCoverageEntity
    {
        public string CoverageFileName { get; set; }

        public string Repository { get; set; }

        public string Project { get; set; }

        public string OutputPath { get; set; }

        /// <summary>
        /// Name With Path
        /// </summary>
        public string CoverageFile
        {
            get { return Path.Combine(this.OutputPath, this.CoverageFileName); }
        }
    }
}