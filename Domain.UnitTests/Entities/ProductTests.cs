using Domain.UnitTests.Fixtures;
using Domain.UnitTests.Fixtures.Generators;
using FluentAssertions;

namespace Domain.UnitTests.Entities;
public class ProductModelTests
{
    [Theory]
    [MemberData(nameof(NumbersForArithmeticOperationsGenerator.GenerateNumbersForSum),
        MemberType = typeof(NumbersForArithmeticOperationsGenerator))]
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
    [MemberData(nameof(NumbersForArithmeticOperationsGenerator.GenerateNumbersForSubtraction),
        MemberType = typeof(NumbersForArithmeticOperationsGenerator))]
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