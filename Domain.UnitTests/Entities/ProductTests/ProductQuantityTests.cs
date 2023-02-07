using Domain.UnitTests.Fixtures;
using Domain.UnitTests.Generators;
using FluentAssertions;

namespace Domain.UnitTests.Entities.ProductTests;

public class ProductQuantityTests
{
    [Theory]
    [MemberData(nameof(NumbersForArithmeticOperationsGenerator.GenerateNumbersForSum),
        MemberType = typeof(NumbersForArithmeticOperationsGenerator))]
    public void Should_SumNumbers_When_IncreaseQuantityBy(int initialQuantity, int increaseBy)
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct(initialQuantity);
        //Act
        sut.IncreaseQuantityBy(increaseBy);
        //Assert
        sut.Quantity.Value.Should().Be(initialQuantity + increaseBy);
    }

    [Theory]
    [MemberData(nameof(NumbersForArithmeticOperationsGenerator.GenerateNumbersForSubtraction),
        MemberType = typeof(NumbersForArithmeticOperationsGenerator))]
    public void Should_SubtractNumbers_When_DecreaseQuantityBy(int initialQuantity, int decreaseBy)
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct(initialQuantity);
        //Act
        sut.DecreaseQuantityBy(decreaseBy);
        //Assert
        sut.Quantity.Value.Should().Be(initialQuantity - decreaseBy);
    }

    [Fact]
    public void Should_ThrowException_When_IncreaseQuantityIsCalledWithNegativeNumber()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct(30);

        //Act && Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.IncreaseQuantityBy(-6));
    }
}