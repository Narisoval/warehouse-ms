using Domain.Primitives;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public class PriceTests
{
    private static readonly Range<decimal> PriceRange = new Price().Range;
    
    [Fact]
    private void Should_ThrowException_When_PriceIsMoreThanMaximum()
    {
        //Arrange
        decimal price = PriceRange.Max + 1M;
        
        //Act & Assert 
        Assert.Throws<ArgumentOutOfRangeException>(() => Price.From(price));
    }

    [Fact]
    private void Should_ThrowException_When_PriceIsLessThenMinimum()
    {
        //Arrange
        decimal price = PriceRange.Min - 0.001M;
        
        //Act & Assert 
        Assert.Throws<ArgumentOutOfRangeException>(() => Price.From(price));
    }
    
    [Fact]
    private void Should_CreatePrice_WhenPriceIsInValidRange()
    {
        //Arrange
        Random rnd = new Random();
        decimal price = PriceRange.Max - PriceRange.Min;
        
        //Act 
        var sut = Price.From(price);
        
        //Assert
        sut.Value.Should().Be(price);
    }
}