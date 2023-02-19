using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class ProductTests
{
    [Fact]
    public void Should_ChangeAllImages_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var newImages = ProductImagesFixture.GetTestProductImages();

        //Act 
        sut.ChangeAllImages(newImages.Value);

        //Assert
        sut.Images.Should().BeEquivalentTo(newImages.Value);
    }

    [Fact]
    public void Should_SetImagesNull_When_ChangeAllImagesArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        IList<ProductImage>? newProductImages = null;
        
        //Act 
        sut.ChangeAllImages(newProductImages);
        
        // Assert 
        sut.Images.Should().BeNull();

    }
    
    [Fact]
    public void CreateProduct_ShouldCreateProductWithSpecifiedValues()
    {
        // Arrange
        var productFormFixture = ProductsFixture.GetTestProduct();

        // Act
        var createdProduct = Product.Create(
            Guid.NewGuid(), 
            productFormFixture.Name,
            productFormFixture.Quantity, 
            productFormFixture.FullPrice, 
            ProductImages.From(productFormFixture.Images!), 
            productFormFixture.Description,
            productFormFixture.IsActive, 
            productFormFixture.Sale, 
            productFormFixture.Provider, 
            productFormFixture.Brand,
            productFormFixture.Category);

        // Assert
        createdProduct.Id.Should().Be(productFormFixture.Id);
        AssertProductsPropertiesAreEqual(createdProduct,productFormFixture);
    }

    [Fact]
    public void CreateProduct_ShouldCreateProductCreateMethodWithIdIsCalled()
    {
        // Arrange
        var productFormFixture = ProductsFixture.GetTestProduct();

        // Act
        var createdProduct = Product.Create(
            productFormFixture.Name,
            productFormFixture.Quantity, 
            productFormFixture.FullPrice, 
            ProductImages.From(productFormFixture.Images!), 
            productFormFixture.Description,
            productFormFixture.IsActive, 
            productFormFixture.Sale, 
            productFormFixture.ProviderId, 
            productFormFixture.BrandId,
            productFormFixture.CategoryId);

        // Assert
        createdProduct.Id.Should().NotBe(Guid.Empty);
        AssertProductsPropertiesAreEqual(createdProduct,productFormFixture);
    }
    
    private void AssertProductsPropertiesAreEqual(Product product1, Product product2)
    {
        product1.Name.Should().Be(product2.Name);
        product1.Quantity.Should().Be(product2.Quantity);
        product1.FullPrice.Should().Be(product2.FullPrice);
        product1.Images.Should().BeEmpty();
        product1.Description.Should().Be(product2.Description);
        product1.IsActive.Should().Be(product2.IsActive);
        product1.Sale.Should().Be(product2.Sale);
        product1.ProviderId.Should().Be(product2.ProviderId);
        product1.BrandId.Should().Be(product2.BrandId);
        product1.CategoryId.Should().Be(product2.CategoryId);
    }
}