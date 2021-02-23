using System.Collections.Generic;
using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface ICoverageLogger
    {
        void Log(IList<CoverageEntity> methodList);
    }
}