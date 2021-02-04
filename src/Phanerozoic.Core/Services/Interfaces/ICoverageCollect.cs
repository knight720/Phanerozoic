using System.Collections.Generic;
using Phanerozoic.Core.Entities;

namespace Phanerozoic.Core.Services.Interfaces
{
    public interface ICoverageCollect
    {
        /// <summary>
        /// 將同一個 Solution 不同 Test Project 的 Coverage 合併
        /// </summary>
        /// <param name="coverageEntity">The coverage entity.</param>
        /// <param name="methodList">The method list.</param>
        /// <returns></returns>
        string Collect(CoreMethodCoverageEntity coverageEntity, IList<CoverageEntity> methodList);

        /// <summary>
        /// Loads the collect.
        /// </summary>
        /// <param name="coverageEntity">The coverage entity.</param>
        /// <returns></returns>
        IList<CoverageEntity> LoadCollect(CoreMethodCoverageEntity coverageEntity);
    }
}