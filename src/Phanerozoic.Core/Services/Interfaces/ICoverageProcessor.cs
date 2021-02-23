using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface ICoverageProcessor
    {
        void Process(ReportEntity reportEntity, CoreMethodCoverageEntity coverageEntity);

        void ProcessParserAndCollect(ReportEntity reportEntity, CoreMethodCoverageEntity coverageEntity);

        void ProcessUpdateAndNotify(CoreMethodCoverageEntity coverageEntity);
    }
}