using Phanerozoic.Core.Entities;
using System.Collections.Generic;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface INotifyer
    {
        void Notify(RepositoryCoverageEntity coverageEntity, IList<CoverageEntity> methodList);
    }
}