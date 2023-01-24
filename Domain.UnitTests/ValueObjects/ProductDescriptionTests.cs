using Domain.Primitives;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public sealed class ProductDescriptionTests
{
    private readonly Range<int> _lengthRange = new ProductDescription().LengthRange;

    [Fact]
    public void Should_ThrowException_When_ProductDescriptionIsNull()
    {
        //Arrange 
        string? description = null;
        
        //Act Assert & Act Assert
        Assert.Throws<ArgumentNullException>(() => ProductDescription.From(description));
    }
    
    [Fact]
    public void Should_ThrowException_When_ProductDescriptionLengthIsLessThanMinimum()
    {
        //Arrange
        var description = new string('a', _lengthRange.Min - 1);
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductDescription.From(description));
    }
    
    [Fact]
    public void Should_ThrowException_When_ProductDescriptionLengthIsMoreThanMaximum()
    {
        //Arrange
        var description = new string('a', _lengthRange.Max + 1);
        
        //Act & Assert 
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductDescription.From(description));
    }
    
    [Fact]
    public void Should_CreateProductDescription_When_ProductDescriptionLengthIsValid()
    {
        //Arrange
        var rnd = new Random();
        var descriptionLength = rnd.Next(_lengthRange.Min, _lengthRange.Max);
        var description = new string('a',descriptionLength);
        
        //Act 
        var sut = ProductDescription.From(description);
            
        //Assert
        sut.Value.Should().Be(description);
    }
}