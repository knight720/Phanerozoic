using FluentAssertions;
using Xunit;

namespace Phanerozoic.Core.Entities.Tests
{
    public class CoverageEntityTests
    {
        [Fact(DisplayName = "測試 CoverageStatus")]
        public void TestCoverageStatus()
        {
            this.CoverageStatusShouldBe(50, 50, CoverageStatus.Unchange);
            this.CoverageStatusShouldBe(50, 60, CoverageStatus.Up);
            this.CoverageStatusShouldBe(50, 40, CoverageStatus.Down);
            this.CoverageStatusShouldBe(0, 50, CoverageStatus.Up);
            this.CoverageStatusShouldBe(50, 0, CoverageStatus.Down);
            this.CoverageStatusShouldBe(0, 0, CoverageStatus.Unchange);
        }

        [Fact(DisplayName = "測試 LastCoverage")]
        public void TestLastCoverage()
        {
            this.LastCoverageShouldBe(10, 20);
            this.LastCoverageShouldBe(30, 20);
            this.LastCoverageShouldBe(20, 20);
        }

        [Fact(DisplayName = "測試 IsPass")]
        public void TestCoverageIsPass()
        {
            this.CoverageIsPassShouldBe(50, 40, false);
            this.CoverageIsPassShouldBe(50, 50, true);
            this.CoverageIsPassShouldBe(50, 60, true);
        }

        [Fact(DisplayName = "測試 NewTargetCoverage")]
        public void TestNewTargetCoverage()
        {
            this.NewTargetCoverageShouldBe(50, 60, 60);
            this.NewTargetCoverageShouldBe(50, 40, 50);
            this.NewTargetCoverageShouldBe(50, 50, 50);
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

        private void LastCoverageShouldBe(int coverage, int newCoverage)
        {
            //// Arrange
            var reportEntity = new CoverageEntity();
            reportEntity.Coverage = newCoverage;

            var target = new CoverageEntity();
            target.Coverage = coverage;

            //// Act
            target.UpdateCoverage(reportEntity);

            //// Assert
            target.LastCoverage.Should().Be(coverage);
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

        private void NewTargetCoverageShouldBe(int targetCoverage, int newCoverage, int result)
        {
            //// Arrange
            var reportEntity = new CoverageEntity();
            reportEntity.Coverage = newCoverage;

            var target = new CoverageEntity();
            target.TargetCoverage = targetCoverage;

            //// Act
            target.UpdateCoverage(reportEntity);
            //// Assert
            target.NewTargetCoverage.Should().Be(result);
        }
    }
}