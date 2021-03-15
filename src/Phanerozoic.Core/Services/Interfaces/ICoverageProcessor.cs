using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface ICoverageProcessor
    {
        void Process(ReportEntity reportEntity, RepositoryCoverageEntity coverageEntity);

        void ProcessParserAndCollect(ReportEntity reportEntity, RepositoryCoverageEntity coverageEntity);

        void ProcessUpdateAndNotify(RepositoryCoverageEntity coverageEntity);
    }
}