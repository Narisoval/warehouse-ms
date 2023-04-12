using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.IntegrationTests.Helpers;
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Infrastructure.Interfaces;

namespace Infrastructure.IntegrationTests.RepositoryTests;

public class ProductRepositoryTests : RepositoryTestsBase<Product,IProductRepository>
{
    public ProductRepositoryTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Should_GetProductWithAllRelatedEntities_When_GettingAllProducts()
    {
        //Act
        const int pageIndex = 1;
        const int pageSize = 1;
        
        var (products,_) = await Repository.GetAll(pageIndex,pageSize);
        
        //Assert
        foreach (var product in products)
        {
            AssertProductHasCorrectNavigationalProperties(product);
        }
    }

    private void AssertProductHasCorrectNavigationalProperties(Product? product)
    {
        product.Should().NotBeNull();
        
        product!.Brand.Should().NotBeNull();
        product.Category.Should().NotBeNull();
        product.Provider.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Should_GetProductWithAllRelatedEntities_When_GettingProductById()
    {
        //Arrange
        var seededProductId = GetRandomSeededEntity().Id;
        
        //Act
        var productFromDb = await Repository.Get(seededProductId);
        
        //Assert
        AssertProductHasCorrectNavigationalProperties(productFromDb);
    }
    
    [Fact]
    public async Task Should_NotAddProduct_When_ForeignKeysAreWrong()
    {
        //Assert 
        var productFromDb = GetRandomSeededEntity();
        
        var productWithWrongProviderId = ProductHelpers.GetNewProductWithChangedForeignKeys(productFromDb, providerId: Guid.NewGuid());
        
        var productWithWrongBrandId = ProductHelpers.GetNewProductWithChangedForeignKeys(productFromDb, brandId: Guid.NewGuid());
        
        var productWithWrongCategoryId = ProductHelpers.GetNewProductWithChangedForeignKeys(productFromDb, categoryId: Guid.NewGuid());
         
        //Act
        await Repository.Add(productWithWrongProviderId);
        await Repository.Add(productWithWrongBrandId);
        await Repository.Add(productWithWrongCategoryId);

        var productFromDbWithWrongProviderId = await Repository.Get(productWithWrongProviderId.Id);
        var productFromDbWithWrongBrandId = await Repository.Get(productWithWrongBrandId.Id);
        var productFromDbWithWrongCategoryId = await Repository.Get(productWithWrongCategoryId.Id);
        
        //Assert
        productFromDbWithWrongProviderId.Should().BeNull();
        productFromDbWithWrongBrandId.Should().BeNull();
        productFromDbWithWrongCategoryId.Should().BeNull();
    }
    
    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[] { null, null, Guid.NewGuid(), 1 },
            new object[] { null, Guid.NewGuid(), Guid.NewGuid(), 2 },
            new object[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 3 }
        };

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task Should_ReturnRightAmountOfErrors_When_AddingProductWithWrongForeignKeys
    (Guid? providerId, Guid? brandId, Guid? categoryId, int expectedErrorCount)
    {
        // Arrange
        var productFromDb = GetRandomSeededEntity();
        var productWrongForeignKeys = ProductHelpers.GetNewProductWithChangedForeignKeys
            (productFromDb, providerId, brandId, categoryId);

        // Act
        var result = await Repository.Add(productWrongForeignKeys);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(expectedErrorCount);
    }
    
    [Fact]
    public override async Task Should_UpdateEntity_When_Called()
    {
        // Arrange
        var product = GetRandomSeededEntity();

        var updatedProduct = Product.Create(
            id: product.Id,
            productName: ProductName.From("Updated Product").Value,
            quantity: Quantity.From(50).Value,
            fullPrice: Price.From(25).Value,
            mainImage: Image.From("https://updatedimage.png").Value,
            images: new List<ProductImage>
            {
                ProductImage.Create(Image.From("https://newimage.png").Value).Value
            },
            productDescription: ProductDescription.From("Updated description of a marvelous product").Value,
            isActive: false,
            sale: Sale.From(10).Value,
            providerId: product.ProviderId,
            brandId: product.BrandId,
            categoryId: product.CategoryId
        ).Value;
        
        // Act
        var result = await Repository.Update(updatedProduct);
        await UnitOfWork.Complete();
        var fetchedProduct = await Repository.Get(product.Id);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        fetchedProduct.Should().BeEquivalentTo(updatedProduct);
    }
    
    [Fact]
    public override async Task Should_ReturnFalse_When_UpdatingNonExistingEntity()
    {
        // Arrange
        var existingProduct = GetRandomSeededEntity();
        var nonExistingProduct = Product.Create(
            Guid.NewGuid(),
            existingProduct.Name,
            existingProduct.Quantity,
            existingProduct.FullPrice,
            existingProduct.MainImage,
            existingProduct.Images,
            existingProduct.Description,
            existingProduct.IsActive,
            existingProduct.Sale,
            existingProduct.ProviderId,
            existingProduct.BrandId,
            existingProduct.CategoryId
        ).Value;

        // Act
        var result = await Repository.Update(nonExistingProduct);
        await UnitOfWork.Complete();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task Should_ReturnRightAmountOfErrors_When_UpdatingProductWithWrongForeignKeys
    (Guid? providerId, Guid? brandId, Guid? categoryId, int expectedErrorCount)
    {
        // Arrange
        var productFromDb = GetRandomSeededEntity();
        var productWrongForeignKeys = ProductHelpers.GetNewProductWithChangedForeignKeys
            (productFromDb, providerId, brandId, categoryId);

        // Act
        var result = await Repository.Update(productWrongForeignKeys);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(expectedErrorCount);
    }
    
    [Fact]
    public override async Task Should_NotChangeAnything_When_UpdatingEntityWithNoChanges()
    {
        // Arrange
        var product = GetRandomSeededEntity();

        // Act
        var result = await Repository.Update(product);
        await UnitOfWork.Complete();
        
        var fetchedProduct = await Repository.Get(product.Id);
         
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        fetchedProduct.Should().Be(product);
    }
}