using Phanerozoic.Core.Entities;
using System.Collections.Generic;

namespace Phanerozoic.Core.Services
{
    public interface IReportParser
    {
        IList<MethodEntity> Parser(CoreMethodCoverageEntity coverageEntity, ReportEntity reportEntity);
    }
}