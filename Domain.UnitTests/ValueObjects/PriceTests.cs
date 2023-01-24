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
    
    [Theory]
    [MemberData(nameof(GetPriceData))]
    private void Should_CreatePrice_WhenPriceIsInValidRange(decimal price)
    {
        //Act 
        var sut = Price.From(price);
        
        //Assert
        sut.Value.Should().Be(price);
    }

    private static IEnumerable<object[]> GetPriceData()
    {
        yield return ConvertToObjectsArray( PriceRange.Max - PriceRange.Min);
        yield return ConvertToObjectsArray(PriceRange.Min * 2);
        yield return ConvertToObjectsArray( PriceRange.Max - 0.1M);
        yield return ConvertToObjectsArray(30);
    }

    private static object[] ConvertToObjectsArray(object obj) => new[] { obj };
}