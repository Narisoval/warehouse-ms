using Domain.UnitTests.Fixtures;
using FluentAssertions;
using Infrastructure.Data;
using Org.BouncyCastle.Crypto.Engines;

namespace Infrastructure.IntegrationTests;

public class ProductCreationTests : IClassFixture<WarehouseDbContextGenerator>
{
    private readonly UnitOfWork _unitOfWork;

    public ProductCreationTests(WarehouseDbContextGenerator generator)
    {
        var dbContext = generator.GetContext();
        _unitOfWork = new UnitOfWork(dbContext);
    }


    [Fact]
    public async Task Should_ThrowException_When_ProductWithNonExistentBrandIsAdded()
    {
       //Arrange
       var testProduct = ProductsFixture.GetTestProduct();
       await _unitOfWork.Categories.Add(testProduct.Category);
       await _unitOfWork.Providers.Add(testProduct.Provider);
       await _unitOfWork.Complete();

       //Act && Assert
       await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.Products.Add(testProduct));
    }
    
    [Fact]
    public async Task Should_ThrowException_When_ProductWithNonExistentCategoryIsAdded()
    {
       //Arrange
       var testProduct = ProductsFixture.GetTestProduct();
       await _unitOfWork.Brands.Add(testProduct.Brand);
       await _unitOfWork.Providers.Add(testProduct.Provider);
       await _unitOfWork.Complete();
       
       //Act && Assert
       await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.Products.Add(testProduct));
    }
    
    [Fact]
    public async Task Should_ThrowException_When_ProductWithNonExistentProviderIsAdded()
    {
       //Arrange
       var testProduct = ProductsFixture.GetTestProduct();
       await _unitOfWork.Brands.Add(testProduct.Brand);
       await _unitOfWork.Categories.Add(testProduct.Category);
       await _unitOfWork.Complete();
       
       //Act && Assert
       await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.Products.Add(testProduct));
    }
    
    [Fact]
    public async Task Should_AddProduct_When_ProductIsCorrect()
    {
       //Arrange
       var testProduct = ProductsFixture.GetTestProduct();
       await _unitOfWork.Brands.Add(testProduct.Brand);
       await _unitOfWork.Categories.Add(testProduct.Category);
       await _unitOfWork.Providers.Add(testProduct.Provider);
       await _unitOfWork.Complete();
   
       //Act 
       await _unitOfWork.Products.Add(testProduct);
       var entitiesWritten = await _unitOfWork.Complete();
       
       //Assert
       entitiesWritten.Should().BeGreaterThan(0);
    }
}