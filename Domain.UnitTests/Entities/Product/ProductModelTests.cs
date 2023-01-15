using Domain.UnitTests.Entities.Product.Fixtures;
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
        var sut = ProductsFixture.GetProductWithFixedQuantity(initialQuantity);
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
        var sut = ProductsFixture.GetProductWithFixedQuantity(initialQuantity);
        //Act
        sut.DecreaseQuantityBy(decreaseBy);
        //Assert
        sut.Quantity.Value.Should().Be(expectedResult);
    }
}