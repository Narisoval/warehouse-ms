using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities.ProductTests;

public class ProductFullPriceTests
{
    [Fact]
    public void Should_ChangeFullPrice_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();

        //Act 
        var newPrice = Price.From(3050);
        sut.ChangeFullPrice(newPrice);

        //Assert
        sut.FullPrice.Should().BeEquivalentTo(newPrice);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeFullPriceArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();

        //Act && Assert
        Price? newPrice = null;
        Assert.Throws<ArgumentNullException>(() => sut.ChangeFullPrice(newPrice));
    }
}