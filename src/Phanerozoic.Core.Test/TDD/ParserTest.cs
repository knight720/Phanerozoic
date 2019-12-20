using FluentAssertions;
using Phanerozoic.Core.TDD;
using Xunit;

namespace Phanerozoic.Core.Test.TDD
{
    public class ParserTest
    {
        [Fact]
        public void Test_Parser()
        {
            //// Arragne
            var path = "report.json";

            var expect = new CoverageEntity 
            {
                Coverage = 99
            };

            //// Act
            var target = new CoverageParser();
            var actual = target.Parser(path);

            //// Assert
            actual.Should().BeEquivalentTo(expect);
        }
    }
}