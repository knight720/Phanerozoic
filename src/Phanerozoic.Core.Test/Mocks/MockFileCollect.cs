using System.Collections.Generic;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Helpers;
using Phanerozoic.Core.Services.Files;

namespace Phanerozoic.Core.Test.Mocks
{
    public class MockFileCollect : FileCollect
    {
        public MockFileCollect(IDateTimeHelper dateTimeHelper) : base(dateTimeHelper)
        {
        }

        protected override void SaveCoverage(string fileName, IList<CoverageEntity> fileMethodList)
        {
        }
    }
}