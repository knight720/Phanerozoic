using FluentAssertions;
using Xunit;

namespace Phanerozoic.Core.Entities.Tests
{
    public class CoverageEntityTests
    {
        [Fact()]
        public void TestCoverageStatus()
        {
            this.CoverageStatusShouldBe(50, 50, CoverageStatus.Unchange);
            this.CoverageStatusShouldBe(50, 60, CoverageStatus.Up);
            this.CoverageStatusShouldBe(50, 40, CoverageStatus.Down);
            this.CoverageStatusShouldBe(0, 50, CoverageStatus.Up);
            this.CoverageStatusShouldBe(50, 0, CoverageStatus.Down);
            this.CoverageStatusShouldBe(0, 0, CoverageStatus.Unchange);
        }

        [Fact]
        public void TestCoverageIsPass()
        {
            this.CoverageIsPassShouldBe(50, 40, false);
            this.CoverageIsPassShouldBe(50, 50, true);
            this.CoverageIsPassShouldBe(50, 60, true);
        }

        private void CoverageStatusShouldBe(int coverage, int newCoverage, CoverageStatus result)
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

        private void CoverageIsPassShouldBe(int targetCoverage, int newCoverage, bool result)
        {
            //// Arrange
            var reportEntity = new CoverageEntity();
            reportEntity.Coverage = newCoverage;

            var target = new CoverageEntity();
            target.TargetCoverage = targetCoverage;

            //// Act
            target.UpdateCoverage(reportEntity);

            //// Assert
            target.IsPass.Should().Be(result);
        }
    }
}