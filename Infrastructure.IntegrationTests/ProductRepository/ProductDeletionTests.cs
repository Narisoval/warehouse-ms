using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Infrastructure.Data;

namespace Infrastructure.IntegrationTests.ProductRepository;

public class ProductDeletionTests : IClassFixture<WarehouseDbContextGenerator>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly List<Product> _testProducts;

    public ProductDeletionTests(WarehouseDbContextGenerator generator)
    {
        var dbContext = generator.GetContext();
        _unitOfWork = new UnitOfWork(dbContext);
        _testProducts = new List<Product>();
        GenerateTestProducts();
        AddTestProducts(); 
    }
    
    [Fact]
    public void Should_ThrowException_When_DeletingNonExistingProduct()
    {
       //Arrange
       var fakeId = Guid.NewGuid();
       var fakeProduct = ProductsFixture.GetTestProduct(fakeId);
       
       //Act && Assert
       _unitOfWork.Products.Remove(fakeProduct);
    }
    
    //TODO Finish
    private void AddTestProducts()
    {
        _unitOfWork.Products.AddRange(_testProducts);
        _unitOfWork.Complete();
    }

    private void GenerateTestProducts()
    {
        foreach (var _ in Enumerable.Range(0,15))
        {
            _testProducts.Add(ProductsFixture.GetTestProduct());     
        }
    }
}