using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductsFixture
{
    private static readonly ProductName TestName = ProductName.From("Kingston 3200 TB hard drive").Value;
    
    private static readonly Quantity TestQuantity = Quantity.From(300).Value;
    
    private static readonly Price TestPrice = Price.From(500.99M).Value;
    
    private static readonly ProductDescription TestProductDescription =
        ProductDescription.From(new string('1', 50)).Value;
    
    private static readonly bool TestIsActive = true;
    
    private static readonly Sale TestSale = Sale.From(0).Value;
    
    private static Guid _testId = Guid.NewGuid();
    private static ProductImages _testProductImages = ProductImagesFixture.GetTestProductImages();
    private static Guid _testBrandId = Guid.NewGuid();
    private static Guid _testCategoryId = Guid.NewGuid();
    private static Guid _testProviderId = Guid.NewGuid();

    private static void ResetGuids()
    {
        _testId = Guid.NewGuid();
        _testProductImages = ProductImagesFixture.GetTestProductImages();
        _testBrandId = Guid.NewGuid();
        _testCategoryId = Guid.NewGuid();
        _testProviderId = Guid.NewGuid(); 
    }
    
    public static Product GetTestProduct()
    {
        ResetGuids();
        return Product.Create(
            _testId,
            TestName,
            TestQuantity,
            TestPrice,
            _testProductImages,
            TestProductDescription,
            TestIsActive,
            TestSale,
            _testProviderId,
            _testBrandId,
            _testCategoryId).Value;
    }
    
    public static Product GetTestProduct(int intQuantity)
    {
        ResetGuids();
        
        return Product.Create(_testId,
            TestName,
            Quantity.From(intQuantity).Value,
            TestPrice,
            _testProductImages,
            TestProductDescription,
            TestIsActive,
            TestSale,
            _testProviderId,
            _testBrandId,
            _testCategoryId).Value;
    }
    
    public static Product GetTestProduct(bool isActive)
    {
        ResetGuids();

        return Product.Create(_testId,
            TestName,
            TestQuantity,
            TestPrice,
            _testProductImages,
            TestProductDescription,
            isActive,
            TestSale,
            _testProviderId,
            _testBrandId,
            _testCategoryId).Value;
    }
    
    public static Product GetTestProduct(ProductImages productImages)
    {
        ResetGuids();
        
        return Product.Create(_testId,
            TestName,
            TestQuantity,
            TestPrice,
            productImages,
            TestProductDescription,
            TestIsActive,
            TestSale,
            _testProviderId,
            _testBrandId,
            _testCategoryId).Value;
    }

    public static Product GetTestProduct(Guid id)
    {
         ResetGuids();
         
         return Product.Create(id,
             TestName,
             TestQuantity,
             TestPrice,
             _testProductImages,
             TestProductDescription,
             TestIsActive,
             TestSale,
             _testProviderId,
             _testBrandId,
             _testCategoryId).Value;       
    }
}