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
        Assert.Throws<ArgumentOutOfRangeException>(() => ProductName.From(name));
    }
    
    [Fact]
    public void Should_ThrowException_When_NameIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ProductName.From(null!));
    }

    //Quantity
    [Fact]
    public void Should_ThrowException_When_QuantityIsLessThanZero()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Quantity.From(-1));
    }
    
    //Description
    
    [Theory]
    [MemberData(nameof(Generators.GenerateDescriptions),MemberType = typeof(Generators))]
    public void Should_ThrowException_When_DescriptionIsIncorrectLength(string description)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Description.From(description));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0.001)]
    [InlineData(1_000_000_000_000)]
    private void Should_ThrowException_When_PriceIsIncorrect(decimal price)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Price.From(price));
    }
}