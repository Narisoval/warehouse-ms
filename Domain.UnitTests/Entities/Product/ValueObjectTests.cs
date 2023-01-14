using Domain.Exceptions;
using Domain.UnitTests.Entities.Product.Fixtures;
using Domain.ValueObjects;

namespace Domain.UnitTests.Entities.Product;

public class ValueObjectTests
{
    //Name  
    [Theory]
    [MemberData(nameof(Generators.GenerateProductNames),
        MemberType = typeof(Generators))]
    public void Should_ThrowException_When_NameIsIncorrectLength(string name)
    {
        Assert.Throws<IncorrectLengthException>(() => ProductName.Create(name));
    }
    
    [Fact]
    public void Should_ThrowException_When_NameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ProductName.Create(null!));
    }

    //Quantity
    [Fact]
    public void Should_ThrowException_When_QuantityIsLessThanZero()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Quantity.Create(-1));
    }
    
    //Description
    
    [Theory]
    [MemberData(nameof(Generators.GenerateDescriptions),MemberType = typeof(Generators))]
    public void Should_ThrowException_When_DescriptionIsIncorrectLength(string description)
    {
        Assert.Throws<IncorrectLengthException>(() => Description.Create(description));
    }

    [Fact]
    public void Should_ThrowException_When_DescriptionIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Description.Create(null!));
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(0.001)]
    [InlineData(1_000_000_000_000)]
    private void Should_ThrowException_When_PriceIsIncorrect(decimal price)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Price.Create(price));
    }
}