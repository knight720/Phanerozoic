using Phanerozoic.Core.Entities;
using System.Collections.Generic;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface IReportParser
    {
        IList<CoverageEntity> Parser(RepositoryCoverageEntity coverageEntity, ReportEntity reportEntity);
    }
}