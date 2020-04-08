using System.Collections.Generic;
using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services.Interface
{
    public interface ICoverageReader
    {
        IList<CoverageEntity> GetList();
    }
}