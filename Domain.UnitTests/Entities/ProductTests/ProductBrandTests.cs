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
    public void Should_SetBrandNull_When_ChangeBrandArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        Brand? brand = null;
        
        //Act 
        sut.ChangeBrand(brand);
        
        //Assert
        sut.Brand.Should().BeNull();
    }
}