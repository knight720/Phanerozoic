using FluentAssertions;
using Xunit;

namespace Phanerozoic.Core.Entities.Tests
{
    public class CoverageEntityTests
    {
        [Fact()]
        public void TestCoverageStatus()
        {
            CoverageStatusShouldBe(50,50, CoverageStatus.Unchange);
            CoverageStatusShouldBe(50,60, CoverageStatus.Up);
            CoverageStatusShouldBe(50,40, CoverageStatus.Down);
            CoverageStatusShouldBe(0,50, CoverageStatus.Up);
            CoverageStatusShouldBe(50,0, CoverageStatus.Down);
            CoverageStatusShouldBe(0,0, CoverageStatus.Unchange);
        }



        private static void CoverageStatusShouldBe(int coverage, int newCoverage, CoverageStatus result)
        {
            //// Arrange
            var reportEntity = new CoverageEntity();
            reportEntity.Coverage = newCoverage;

            var target = new CoverageEntity();
            target.Coverage = coverage;

            //// Act
            target.UpdateCoverage(reportEntity);

            //// Assert
            target.Status.Should().Be(result);
        }
    }
}