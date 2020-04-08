using System.Collections.Generic;
using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services
{
    public interface ICoverageUpdater
    {
        IList<MethodEntity> Update(CoreMethodCoverageEntity coverageEntity, IList<MethodEntity> methodList);
    }
}