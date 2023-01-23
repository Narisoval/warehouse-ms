using Domain.Validation;
using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class ProductNameTests
{
    private static readonly Range<int> LengthRange = ProductName.GetRange();
    
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
        var productName = new string('a', LengthRange.Min - 1);
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductName.From(productName));
    }
    
    [Fact]
    public void Should_ThrowException_When_ProductNameLengthIsMoreThanMaximum()
    {
        //Arrange
        var productName = new string('a', LengthRange.Max + 1);
        //Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductName.From(productName));
    }
}