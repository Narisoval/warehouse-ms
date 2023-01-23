using Domain.Validation;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public sealed class ProductDescriptionTests
{
    private static readonly Range<int> LengthRange = ProductDescription.GetRange();

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
        string description = new string('a', LengthRange.Min - 1);
        
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductDescription.From(description));
    }
    
    [Fact]
    public void Should_ThrowException_When_ProductDescriptionLengthIsMoreThanMaximum()
    {
        //Arrange
        string description = new string('a', LengthRange.Max + 1);
        
        //Act & Assert 
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductDescription.From(description));
    }
    
    [Fact]
    public void Should_CreateProductDescription_When_ProductDescriptionLengthIsValid()
    {
        //Arrange
        Random rnd = new Random();
        string description = new string('a',rnd.Next(LengthRange.Min,LengthRange.Max));
        
        //Act 
        var sut = ProductDescription.From(description);
            
        //Assert
        sut.Value.Should().Be(description);
    }

}