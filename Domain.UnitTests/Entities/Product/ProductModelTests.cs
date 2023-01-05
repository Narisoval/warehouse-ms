using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities.Product;

public class ProductModelTests
{
    [Theory]
    [MemberData(nameof(Generators.GenerateNumbersForSum),
        MemberType = typeof(Generators))]
    public void Should_SumNumbers_When_IncreaseQuantityBy(int initialQuantity, int increaseBy, int expectedSum)
    {
        //Arrange
        var sut = new Domain.Entities.Product { Quantity = Quantity.From(initialQuantity) };
        //Act
        sut.IncreaseQuantityBy(increaseBy);
        //Assert
        sut.Quantity.Value.Should().Be(expectedSum);
    }
    
    [Theory]
    [MemberData(nameof(Generators.GenerateNumbersForSubtraction),
        MemberType = typeof(Generators))]
    public void Should_SubtractNumbers_When_DecreaseQuantityBy(int initialQuantity, int decreaseBy, int expectedResult)
    {
        //Arrange
        var sut = new Domain.Entities.Product { Quantity = Quantity.From(initialQuantity) };
        //Act
        sut.DecreaseQuantityBy(decreaseBy);
        //Assert
        sut.Quantity.Value.Should().Be(expectedResult);
    }
}