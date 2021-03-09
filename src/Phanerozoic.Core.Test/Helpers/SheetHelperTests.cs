using FluentAssertions;
using Phanerozoic.Core.Entities;
using Xunit;

namespace Phanerozoic.Core.Helpers.Tests
{
    public class SheetHelperTests
    {
        [Fact(DisplayName = "Sheet 索引值與欄位轉換測試")]
        public void ColumnToLetterTest()
        {
            this.ColumnLetterAssert(1, "A");
            this.ColumnLetterAssert(26, "Z");
            this.ColumnLetterAssert(27, "AA");
            this.ColumnLetterAssert(52, "AZ");
        }

        private void ColumnLetterAssert(int column, string letter)
        {
            //// Target
            var actual = SheetHelper.ColumnToLetter(column);

            //// Assert
            actual.Should().Be(letter);
        }

        [Fact(DisplayName = "Object 轉 Enum")]
        public void ObjectToEnumTest()
        {
            ObjectToEnumAssert(MethodLevel.High.ToString(), MethodLevel.High);
            ObjectToEnumAssert(MethodLevel.Middle.ToString(), MethodLevel.Middle);
            ObjectToEnumAssert(MethodLevel.Low.ToString(), MethodLevel.Low);
            ObjectToEnumAssert(MethodLevel.Middle.ToString().ToLower(), MethodLevel.Middle);
            ObjectToEnumAssert($" {MethodLevel.Middle.ToString().ToLower()} ", MethodLevel.Middle);
            ObjectToEnumAssert($" {MethodLevel.Middle.ToString()} ", MethodLevel.Middle);
            ObjectToEnumAssert(null, MethodLevel.Low);
            ObjectToEnumAssert("ErrorValue", MethodLevel.Low);
        }

        private static void ObjectToEnumAssert(object value, MethodLevel expected)
        {
            //// Act
            var actual = SheetHelper.ObjectToEnum<MethodLevel>(value);

            //// Assert
            actual.Should().Be(expected);
        }
    }
}