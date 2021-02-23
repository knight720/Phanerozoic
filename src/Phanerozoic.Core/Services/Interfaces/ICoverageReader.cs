using System.Collections.Generic;
using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface ICoverageReader
    {
        IList<CoverageEntity> GetList();
    }
}