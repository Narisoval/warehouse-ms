using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities.ProductTests;

public class ProductSaleTests
{
    [Fact]
    public void Should_ChangeSale_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var originalSale = sut.Sale;
        var newSale = Sale.From(37.9M);

        //Act 
        sut.ChangeSale(newSale);

        //Assert
        sut.Sale.Should().NotBe(originalSale);
        sut.Sale.Should().Be(newSale);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeSaleArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        Sale? newSale = null;

        //Act && Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeSale(newSale));
    }
}