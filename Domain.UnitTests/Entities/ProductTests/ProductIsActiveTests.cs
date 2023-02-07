using Domain.UnitTests.Fixtures;
using FluentAssertions;

namespace Domain.UnitTests.Entities.ProductTests;

public class ProductIsActiveTests
{
    [Fact]
    public void Should_SetIsActiveTrue_When_EnableProductIsCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct(false);

        //Act
        sut.EnableProduct();

        //Assert
        sut.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Should_SetIsActiveFalse_When_DisableProductIsCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct(true);

        //Act
        sut.DisableProduct();

        //Assert
        sut.IsActive.Should().BeFalse();
    }
}