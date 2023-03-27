using Domain.Entities;
using Domain.UnitTests.Fixtures;
using static Domain.UnitTests.Fixtures.ProductsFixture;
using Domain.ValueObjects;
using FluentAssertions;
using FluentResults;
namespace Domain.UnitTests.Entities;

public class ProductTests
{
    [Fact]
    public void Should_CreateProduct_When_CreateMethodWithIdIsCalled()
    {
        // Act
        var productResult = Product.Create(
            id: _testId,
            productName: TestName,
            quantity: TestQuantity,
            fullPrice: TestPrice,
            mainImage: TestMainImage,
            images: _testProductImages,
            productDescription: TestProductDescription,
            isActive: TestIsActive,
            sale: TestSale,
            providerId: _testProviderId,
            brandId: _testBrandId,
            categoryId: _testCategoryId);

        // Assert
        AssertProviderCreatedCorrectly(productResult);
        productResult.Value.Id.Should().Be(_testId);
    }

    [Fact]
    public void Should_CreateProduct_When_CreateMethodWithoutIdIsCalled()
    {
        // Act
        var productResult = Product.Create(
            productName: TestName,
            quantity: TestQuantity,
            fullPrice: TestPrice,
            mainImage: TestMainImage,
            images: _testProductImages,
            productDescription: TestProductDescription,
            isActive: TestIsActive,
            sale: TestSale,
            providerId: _testProviderId,
            brandId: _testBrandId,
            categoryId: _testCategoryId);

        // Assert
        AssertProviderCreatedCorrectly(productResult);
        productResult.Value.Id.Should().NotBe(Guid.Empty);
    }
    
    [Fact]
    public void Should_SetProductImages_WhenCalled()
    {
        //Arrange
        var sut = GetTestProduct();
        var newImages = ProductImagesFixture.GetTestProductImages();

        //Act 
        sut.SetProductImages(newImages);

        //Assert
        sut.Images.Should().BeEquivalentTo(newImages);
        sut.Images.Should().NotBeNull();
        foreach (var productImage in sut.Images)
        {
            productImage.ProductId.Should().Be(_testId);
            productImage.Product.Should().Be(sut);
        }
    }

    [Fact]
    public void Should_SetImagesNull_When_SetProductImagesIsNull()
    {
        //Arrange
        var sut = GetTestProduct();
        IReadOnlyCollection<ProductImage>? newProductImages = null;
        
        //Act 
        sut.SetProductImages(newProductImages);
        
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
            productName: TestName,
            quantity: TestQuantity,
            fullPrice: TestPrice,
            mainImage: TestMainImage,
            images: _testProductImages,
            productDescription: TestProductDescription,
            isActive: TestIsActive,
            sale: TestSale,
            providerId: _testProviderId,
            brandId: _testBrandId,
            categoryId: _testCategoryId);
        
        //Assert
        productResult.AssertIsFailed(1);
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
            productName: TestName,
            quantity: TestQuantity,
            fullPrice: TestPrice,
            mainImage: TestMainImage,
            images: _testProductImages,
            productDescription: TestProductDescription,
            isActive: TestIsActive,
            sale: TestSale,
            providerId: providerId,
            brandId: brandId,
            categoryId: categoryId);
        
        //Assert
        productResult.AssertIsFailed(4);
    }

    [Fact]
    public void Should_ReturnFailedResult_When_SomePropertiesAreNull()
    {
        //Arrange
        var nullableArguments = new List<object?>()
        {
            TestName,
            TestQuantity,
            TestPrice,
            TestMainImage,
            _testProductImages,
            TestProductDescription,
            TestSale
        };
        
        for (int i = 0; i < nullableArguments.Count; i++)
        {
            //Act 
            nullableArguments[i] = null;
            var productResultWithId = CreateProduct(nullableArguments,_testId);
            var productResultWithoutId = CreateProduct(nullableArguments);
            
            //Assert
            productResultWithId.AssertIsFailed(i+1);
            productResultWithoutId.AssertIsFailed(i + 1);
        }
    }
    
    private void AssertProviderCreatedCorrectly(Result<Product> productResult)
    {
        productResult.IsSuccess.Should().BeTrue();
        productResult.Value.Id.Should().NotBe(Guid.Empty);
        productResult.Value.Name.Should().Be(TestName);
        productResult.Value.Quantity.Should().Be(TestQuantity);
        productResult.Value.FullPrice.Should().Be(TestPrice);
        productResult.Value.Images.Should().BeEquivalentTo(_testProductImages);
        productResult.Value.Description.Should().Be(TestProductDescription);
        productResult.Value.IsActive.Should().Be(TestIsActive);
        productResult.Value.Sale.Should().BeEquivalentTo(TestSale);
        productResult.Value.ProviderId.Should().Be(_testProviderId);
        productResult.Value.BrandId.Should().Be(_testBrandId);
        productResult.Value.CategoryId.Should().Be(_testCategoryId);
    }
    
    private Result<Product> CreateProduct(List<object?> arguments, Guid? id = null)
    {
        var productName = (ProductName?)arguments[0];
        var quantity = (Quantity?)arguments[1];
        var price = (Price?)arguments[2];
        var mainImage = (Image?)arguments[3];
        var images = (IReadOnlyCollection<ProductImage>?)arguments[4];
        var description = (ProductDescription?)arguments[5];
        var sale = (Sale?)arguments[6];
        if (id == null)
        {
            return Product.Create(
                productName: productName,
                quantity: quantity,
                fullPrice: price,
                mainImage: mainImage,
                images: images,
                productDescription: description,
                isActive: true,
                sale: sale,
                providerId: _testProviderId,
                brandId: _testBrandId,
                categoryId: _testCategoryId);
        }
        
        return Product.Create(
            id: id.Value,
            productName: productName,
            quantity: quantity,
            fullPrice: price,
            mainImage: mainImage,
            images: images,
            productDescription: description,
            isActive: true,
            sale: sale,
            providerId: _testProviderId,
            brandId: _testBrandId,
            categoryId: _testCategoryId);
    }
    
}