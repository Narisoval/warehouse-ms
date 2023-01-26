using Domain.Primitives;
using FluentAssertions;

namespace Domain.UnitTests.Foundation;

public class StringValueObjectWithLengthRestrictionsTests
{
    private static readonly Range<int> LengthRange = Range<int>.Create(30, 60);
    [Fact]
    public void Should_ThrowException_When_StringIsNull()
    {
        //Arrange
        string? testString = null;
        
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => TestStringValueObjectWithLengthRestrictions.From(testString));
    }


    [Fact]
    public void Should_ThrowException_When_StringLengthIsMoreThanMaximum()
    {
        //Arrange    
        var stringLength = LengthRange.Max + 1;
        var testString = new string('*', stringLength);
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => TestStringValueObjectWithLengthRestrictions.From(testString));
    }
    
    [Fact]
    public void Should_ThrowException_When_StringLengthIsLessThemMinimum()
    {
        //Arrange    
        var stringLength = LengthRange.Min - 1;
        var testString = new string('*', stringLength);
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => TestStringValueObjectWithLengthRestrictions.From(testString));
    }
    
    [Fact]
    public void Should_CreateProperObject_When_StringLengthIsInTheCorrectRange()
    {
        //Arrange    
        var stringLength = LengthRange.Max - LengthRange.Min;
        var testString = new string('*', stringLength);
        
        //Act 
        var sut = TestStringValueObjectWithLengthRestrictions.From(testString);
        
        //Assert
        sut.Value.Should().BeEquivalentTo(testString);
    }
    
    private class TestStringValueObjectWithLengthRestrictions : 
        StringValueObjectWithLengthRestrictions<TestStringValueObjectWithLengthRestrictions>
    {
        internal override Range<int> LengthRange => StringValueObjectWithLengthRestrictionsTests.LengthRange;
    }
}