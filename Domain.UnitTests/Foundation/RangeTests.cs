using Domain.Primitives;
using FluentAssertions;

namespace Domain.UnitTests.Foundation;

public class RangeTests
{
    [Fact]
    public void Should_ThrowException_When_MaxIsLessThenMin()
    {
        //Arrange
        int min = 44;
        int max = 0;
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Range<int>.Create(min, max));
    }
    
    [Fact]
    public void Should_CreateRange_WhenMaxIsLessThanMin()
    {
        //Arrange
        var min = 0;
        var max = 40;
        
        //Act
        var sut = Range<int>.Create(min, max);
        
        //Assert
        sut.Min.Should().Be(min);
        sut.Max.Should().Be(max);
    }
    
    [Fact]
    public void Should_CreateRange_WhenMaxIsEqualToMin()
    {
        //Arrange
        var min = 40;
        
        //Act
        var sut = Range<int>.Create(min, min);
        
        //Assert
        sut.Min.Should().Be(min);
        sut.Max.Should().Be(min);
    }
}