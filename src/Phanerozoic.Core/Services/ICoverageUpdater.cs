using System.Collections.Generic;
using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services
{
    public interface ICoverageUpdater
    {
        IList<CoverageEntity> Update(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList);
    }
}