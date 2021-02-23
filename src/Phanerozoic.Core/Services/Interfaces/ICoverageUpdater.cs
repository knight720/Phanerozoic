using System.Collections.Generic;
using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface ICoverageUpdater
    {
        IList<CoverageEntity> Update(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList);
    }
}