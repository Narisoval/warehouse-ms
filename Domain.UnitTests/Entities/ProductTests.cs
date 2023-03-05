using Domain.Entities;
using Domain.Primitives;
using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;
using FluentResults;

namespace Domain.UnitTests.Entities;

public class ProductTests
{
    private readonly ProductName _name = ProductName.From("Kingston 3200 TB hard drive").Value;
    
    private readonly Quantity _quantity = Quantity.From(300).Value;
    
    private readonly Price _price = Price.From(500.99M).Value;
    
    private readonly ProductDescription _productDescription =
        ProductDescription.From(new string('1', 50)).Value;
    
    private readonly bool _isActive = true;
    
    private readonly Sale _sale = Sale.From(0).Value;
    
    private readonly Guid _id = Guid.NewGuid();
    private readonly ProductImages _productImages = ProductImagesFixture.GetTestProductImages();
    private readonly Guid _brandId = Guid.NewGuid();
    private readonly Guid _categoryId = Guid.NewGuid();
    private readonly Guid _providerId = Guid.NewGuid();
    
    [Fact]
    public void Should_CreateProduct_When_CreateMethodWithIdIsCalled()
    {
        // Act
        var productResult = Product.Create(
            id: _id,
            productName: _name,
            quantity: _quantity,
            fullPrice: _price,
            images: _productImages,
            productDescription: _productDescription,
            isActive: _isActive,
            sale: _sale,
            providerId: _providerId,
            brandId: _brandId,
            categoryId: _categoryId);

        // Assert
        AssertProviderCreatedCorrectly(productResult);
        productResult.Value.Id.Should().Be(_id);
    }

    [Fact]
    public void Should_CreateProduct_When_CreateMethodWithoutIdIsCalled()
    {
        // Act
        var productResult = Product.Create(
            productName: _name,
            quantity: _quantity,
            fullPrice: _price,
            images: _productImages,
            productDescription: _productDescription,
            isActive: _isActive,
            sale: _sale,
            providerId: _providerId,
            brandId: _brandId,
            categoryId: _categoryId);

        // Assert
        AssertProviderCreatedCorrectly(productResult);
    }
    
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
    public void Should_ReturnFailedResult_When_IdIsEmptyGuid()
    {
        //Arrange
        var id = Guid.Empty;
        
        //Act
        var productResult = Product.Create(
            id,
            productName: _name,
            quantity: _quantity,
            fullPrice: _price,
            images: _productImages,
            productDescription: _productDescription,
            isActive: _isActive,
            sale: _sale,
            providerId: _providerId,
            brandId: _brandId,
            categoryId: _categoryId);
        
        //Assert
        productResult.IsFailed.Should().BeTrue();
        productResult.Errors.Count.Should().Be(1);
    }
    
    [Fact]
    public void Should_ReturnResultWithRightAmountOfErrors_When_AllIdsAreEmptyGuids()
    {
        //Arrange
        var id = Guid.Empty;
        var providerId = Guid.Empty;
        var brandId = Guid.Empty;
        var categoryId = Guid.Empty;
        
        //Act
        var productResult = Product.Create(
            id,
            productName: _name,
            quantity: _quantity,
            fullPrice: _price,
            images: _productImages,
            productDescription: _productDescription,
            isActive: _isActive,
            sale: _sale,
            providerId: providerId,
            brandId: brandId,
            categoryId: categoryId);
        
        //Assert
        productResult.IsFailed.Should().BeTrue();
        productResult.Errors.Count.Should().Be(4);
    }

    [Fact]
    public void Should_ReturnFailedResult_When_SomePropertiesAreNull()
    {
        //Arrange
        var valueObjects = new List<ValueObject?>()
        {
            _name,
            _quantity,
            _price,
            _productDescription,
            _sale
        };
        
        for (int i = 0; i < valueObjects.Count; i++)
        {
            //Act 
            valueObjects[i] = null;
            var productResultWithId = CreateProductWithId(valueObjects);
            var productResultWithoutId = CreatProductWithoutId(valueObjects);
            
            //Assert
            productResultWithId.AssertIsFailed(i+1);
            productResultWithoutId.AssertIsFailed(i + 1);
        }
    }
    
    private void AssertProviderCreatedCorrectly(Result<Product> productResult)
    {
        productResult.IsSuccess.Should().BeTrue();
        productResult.Value.Id.Should().NotBe(Guid.Empty);
        productResult.Value.Name.Should().Be(_name);
        productResult.Value.Quantity.Should().Be(_quantity);
        productResult.Value.FullPrice.Should().Be(_price);
        productResult.Value.Images.Should().BeEquivalentTo(_productImages.Value);
        productResult.Value.Description.Should().Be(_productDescription);
        productResult.Value.IsActive.Should().Be(_isActive);
        productResult.Value.Sale.Should().BeEquivalentTo(_sale);
        productResult.Value.ProviderId.Should().Be(_providerId);
        productResult.Value.BrandId.Should().Be(_brandId);
        productResult.Value.CategoryId.Should().Be(_categoryId);
    }
    
    private Result<Product> CreateProductWithId(List<ValueObject?> arguments)
    {
        return Product.Create(
            id: _id,
            productName: (ProductName?)arguments[0],
            quantity: (Quantity?)arguments[1]!,
            fullPrice: (Price?)arguments[2],
            images: _productImages,
            productDescription: (ProductDescription?)arguments[3],
            isActive: true,
            sale: (Sale?)arguments[4],
            providerId: _providerId,
            brandId: _brandId,
            categoryId: _categoryId);
    }
    
    private Result<Product> CreatProductWithoutId(List<ValueObject?> arguments)
    {
        return Product.Create(
            productName: (ProductName?)arguments[0],
            quantity: (Quantity?)arguments[1]!,
            fullPrice: (Price?)arguments[2],
            images: _productImages,
            productDescription: (ProductDescription?)arguments[3],
            isActive: true,
            sale: (Sale?)arguments[4],
            providerId: _providerId,
            brandId: _brandId,
            categoryId: _categoryId);
    }
}