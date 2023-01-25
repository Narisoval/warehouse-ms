using Domain.Primitives;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public class ProductNameTests
{
    private readonly Range<int> _lengthRange = new ProductName().LengthRange;
    
    [Fact]
    public void Should_ThrowException_When_ProductNameIsNull()
    {
        //Arrange
        string? productName = null;
        
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => ProductName.From(productName));
    }
    
    [Fact]
    public void Should_ThrowException_When_ProductNameLengthIsLessThanMinimum()
    {
        //Arrange
        var productName = new string('a', _lengthRange.Min - 1);
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductName.From(productName));
    }
    
    [Fact]
    public void Should_ThrowException_When_ProductNameLengthIsMoreThanMaximum()
    {
        //Arrange
        var productName = new string('a', _lengthRange.Max + 1);
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductName.From(productName));
    }

    [Fact]
    public void Should_CreateProductName_When_LengthIsInCorrectRange()
    {
        //Arrange
        var rnd = new Random();
        var nameLength = rnd.Next(_lengthRange.Min, _lengthRange.Max);
        var productName = new string('a',nameLength);
        
        //Act 
        var sut = ProductName.From(productName);
            
        //Assert
        sut.Value.Should().Be(productName);
    }
}