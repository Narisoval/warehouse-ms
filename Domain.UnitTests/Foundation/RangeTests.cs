using Domain.Primitives;
using FluentAssertions;

namespace Domain.UnitTests.Foundation;

public class RangeTests
{
    [Fact]
    public void Should_ThrowException_When_MaxIsLessThenMin()
    {
        //Arrange
        const int min = 44;
        const int max = 0;
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => Range<int>.Create(min, max));
    }
    
    [Fact]
    public void Should_CreateRange_WhenMaxIsLessThanMin()
    {
        //Arrange
        const int min = 0;
        const int max = 40;
        
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
        const int min = 40;
        
        //Act
        var sut = Range<int>.Create(min, min);
        
        //Assert
        sut.Min.Should().Be(min);
        sut.Max.Should().Be(min);
    }
}