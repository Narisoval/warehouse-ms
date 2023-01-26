using Domain.Primitives;
using FluentAssertions;

namespace Domain.UnitTests.Foundation;

public class NumericValueObjectInRangeTests
{
    private static readonly Range<int> Range = Range<int>.Create(20, 100);

    [Fact]
    public void Should_ThrowException_When_NumberIsOutOfRange()
    {
        //Arrange
        int numLessThanMin = Range.Min - 1;
        int numMoreThanMax = Range.Max + 1;
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => TestNumericValueObjectInRange.From(numLessThanMin));
        Assert.Throws<ArgumentOutOfRangeException>(() => TestNumericValueObjectInRange.From(numMoreThanMax));
    }
    
    [Fact]
    public void Should_CreateObject_When_NumberIsInRange()
    {
        //Arrange
        int testValue = Range.Max - Range.Min;
        
        //Act
        var sut = TestNumericValueObjectInRange.From(testValue);

        //Assert
        sut.Value.Should().Be(testValue);
    }
    
    private class TestNumericValueObjectInRange : NumericValueObjectInRange<int,TestNumericValueObjectInRange>
    {
        internal override Range<int> Range => NumericValueObjectInRangeTests.Range;
    }
}