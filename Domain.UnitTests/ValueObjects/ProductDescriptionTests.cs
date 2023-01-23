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
        string description = new string('a', _lengthRange.Min - 1);
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductDescription.From(description));
    }
    
    [Fact]
    public void Should_ThrowException_When_ProductDescriptionLengthIsMoreThanMaximum()
    {
        //Arrange
        string description = new string('a', _lengthRange.Max + 1);
        
        //Act & Assert 
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductDescription.From(description));
    }
    
    [Fact]
    public void Should_CreateProductDescription_When_ProductDescriptionLengthIsValid()
    {
        //Arrange
        Random rnd = new Random();
        string description = new string('a',rnd.Next(_lengthRange.Min,_lengthRange.Max));
        
        //Act 
        var sut = ProductDescription.From(description);
            
        //Assert
        sut.Value.Should().Be(description);
    }

}