using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Infrastructure.Data;

namespace Infrastructure.IntegrationTests.ProductRepository;

public class ProductRetrievalTests : IClassFixture<WarehouseDbContextGenerator>,IAsyncLifetime
{
    private readonly UnitOfWork _unitOfWork;
    private readonly List<Product> _testProducts;
    private const int ProductsAmount = 15;

    public ProductRetrievalTests(WarehouseDbContextGenerator generator)
    {
        var dbContext = generator.GetContext();
        _unitOfWork = new UnitOfWork(dbContext);
        _testProducts = new List<Product>();
        GenerateTestProducts();
    }

    [Fact]
    public async Task Should_RetrieveProductWithCorrectData_When_GetWithProviderIsCalled()
    {
        //Arrange
        var randomProductIndex = new Random().Next(0, ProductsAmount);
        var productFromFixture = _testProducts[randomProductIndex];
        
        //Act
        var productFromDb = await _unitOfWork.Products.GetProductWithProvider(productFromFixture.Id);
        
        //Assert
        Assert.NotNull(productFromDb);
        
        CheckProductsAreEqual(productFromFixture,productFromDb!,withProvider:true);
    }

    [Fact] 
    public async Task Should_RetrieveAllProductsWithCorrectData_When_GetAllWithProviderIsCalled()
    {
        //Act
        var productsFromDb = await _unitOfWork.Products.GetAllProductsWithProvider();
        
        //Assert
        Assert.NotNull(productsFromDb);
        
        foreach (var productFromDb in productsFromDb)
        {
            var productFromFixture = _testProducts.First(product => product.Id == productFromDb.Id);
            Assert.NotNull(productFromFixture);
            CheckProductsAreEqual(productFromFixture,productFromDb,withProvider:true);
        }
    }
    
    [Fact]
    public async Task Should_RetrieveProductWithCorrectData_When_GetIsCalled()
    {
        //Arrange
        var randomProductIndex = new Random().Next(0, ProductsAmount);
        var productFromFixture = _testProducts[randomProductIndex];
        
        //Act
        var productFromDb = await _unitOfWork.Products.GetProductWithProvider(productFromFixture.Id);
        
        //Assert
        Assert.NotNull(productFromDb);
        
        CheckProductsAreEqual(productFromFixture,productFromDb!,withProvider:false);
    }

    [Fact] 
    public async Task Should_RetrieveAllProductsWithCorrectData_When_GetAllIsCalled()
    {
        //Act
        var productsFromDb = await _unitOfWork.Products.GetAll();
        
        //Assert
        Assert.NotNull(productsFromDb);
        foreach (var productFromDb in productsFromDb)
        {
            var productFromFixture = _testProducts.First(product => product.Id == productFromDb.Id);
            Assert.NotNull(productFromFixture);
            CheckProductsAreEqual(productFromFixture,productFromDb,withProvider:false);
        }
    }

    private void CheckProductsAreEqual(Product product1, Product product2, bool withProvider)
    {
        //Product 
        Assert.Equal(product1.Id, product2.Id);
        Assert.Equal(product1.Name.Value, product2.Name.Value);
        Assert.Equal(product1.Quantity.Value, product2.Quantity.Value);
        Assert.Equal(product1.FullPrice.Value, product2.FullPrice.Value);
        Assert.Equal(product1.Description.Value, product2.Description.Value);
        Assert.Equal(product1.Sale.Value, product2.Sale.Value);
        Assert.Equal(product1.IsActive, product2.IsActive);
        
        //Category
        Assert.Equal(product1.Category?.Id, product2.Category?.Id);
        Assert.Equal(product1.CategoryId, product2.CategoryId);
        Assert.Equal(product1.Category?.Name.Value,product2.Category?.Name.Value);
        
        //Brand
        Assert.Equal(product1.BrandId,product2.BrandId);
        Assert.Equal(product1.Brand?.Id, product2.Brand?.Id);
        Assert.Equal(product1.Brand?.Name.Value, product2.Brand?.Name.Value);
        Assert.Equal(product1.Brand?.Image.Value, product2.Brand?.Image.Value);
        Assert.Equal(product1.Brand?.Description.Value, product2.Brand?.Description.Value);

        if (withProvider)
        {
            //Provider
            Assert.Equal(product1.ProviderId, product2.ProviderId);
            Assert.Equal(product1.Provider?.Id, product2.Provider?.Id);
            Assert.Equal(product1.Provider?.CompanyName.Value, product2.Provider?.CompanyName.Value);
            Assert.Equal(product1.Provider?.PhoneNumber, product2.Provider?.PhoneNumber);
            Assert.Equal(product1.Provider?.Email.Value, product2.Provider?.Email.Value);
        }

        //Images
        Assert.Equal(product1.Images?.Count,product2.Images?.Count);
        Assert.Equal(product1.Images, product2.Images);    
    }
    
    private void GenerateTestProducts()
    {
        foreach (var _ in Enumerable.Range(0,ProductsAmount))
        {
            _testProducts.Add(ProductsFixture.GetTestProduct());     
        }
    }
    
    private async Task AddTestProducts()
    {
        var testBrands = _testProducts.Select(product => product.Brand);
        var testProviders = _testProducts.Select(product => product.Provider);
        var testCategories = _testProducts.Select(product => product.Category);
        await _unitOfWork.Brands.AddRange(testBrands);
        await _unitOfWork.Providers.AddRange(testProviders);
        await _unitOfWork.Categories.AddRange(testCategories);
        await _unitOfWork.Complete();
        
        await _unitOfWork.Products.AddRange(_testProducts);
        await _unitOfWork.Complete();
    }


    public async Task InitializeAsync()
    {
        await AddTestProducts();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        _unitOfWork.Products.RemoveRange(_testProducts);
        await _unitOfWork.Complete();
    }

}