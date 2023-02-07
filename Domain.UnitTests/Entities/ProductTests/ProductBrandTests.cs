using Domain.Entities;
using Domain.UnitTests.Fixtures;
using FluentAssertions;

namespace Domain.UnitTests.Entities.ProductTests;

public class ProductBrandTests
{
    [Fact]
    public void Should_ChangeBrandAndId_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var originalBrandId = sut.BrandId;

        var newBrand = BrandsFixture.GetTestBrand();
        var newBrandId = newBrand.Id;

        //Act 
        sut.ChangeBrand(newBrand);

        //Assert
        sut.Brand.Should().Be(newBrand);
        sut.BrandId.Should().Be(newBrandId);
        sut.BrandId.Should().NotBe(originalBrandId);
    }
    
    [Fact]
    public void Should_ThrowException_When_ChangeBrandArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();

        //Act 
        Brand? brand = null;

        //Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeBrand(brand));
    }
}