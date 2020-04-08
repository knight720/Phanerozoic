using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services
{
    public interface ICoverageProcessor
    {
        void Process(ReportEntity reportEntity, CoreMethodCoverageEntity coverageEntity);
    }
}