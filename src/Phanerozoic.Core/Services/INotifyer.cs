using Phanerozoic.Core.Entities;
using System.Collections.Generic;

namespace Phanerozoic.Core.Services
{
    public interface INotifyer
    {
        void Notify(CoreMethodCoverageEntity coverageEntity, IList<MethodEntity> methodList);
    }
}