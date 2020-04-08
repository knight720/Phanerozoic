using System.Collections.Generic;
using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services
{
    public interface ICoverageLogger
    {
        void Log(IList<CoverageEntity> methodList);
    }
}