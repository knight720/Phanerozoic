using Phanerozoic.Core.Entities;
using System.Collections.Generic;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface INotifyer
    {
        void Notify(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList);
    }
}