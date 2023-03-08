using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductsFixture
{
    public static readonly ProductName TestName = ProductName.From("Kingston 3200 TB hard drive").Value;
    
    public static readonly Quantity TestQuantity = Quantity.From(300).Value;
    
    public static readonly Price TestPrice = Price.From(500.99M).Value;
    
    public static readonly ProductDescription TestProductDescription =
        ProductDescription.From(new string('1', 50)).Value;
    
    public static readonly bool TestIsActive = true;
    
    public static readonly Sale TestSale = Sale.From(0).Value;
    
    public static readonly Image TestMainImage = Image.From("https://mainImage.png").Value;
    
    public static Guid _testId = Guid.NewGuid();
    public static IReadOnlyCollection<ProductImage> _testProductImages = ProductImagesFixture.GetTestProductImages();
    public static Guid _testBrandId = Guid.NewGuid();
    public static Guid _testCategoryId = Guid.NewGuid();
    public static Guid _testProviderId = Guid.NewGuid();

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
            categoryId: _testCategoryId).Value;
    }
    
    public static Product GetTestProduct(int intQuantity)
    {
        ResetGuids();
        return Product.Create(
            id: _testId,
            productName: TestName,
            quantity: Quantity.From(intQuantity).Value,
            fullPrice: TestPrice,
            mainImage: TestMainImage,
            images: _testProductImages,
            productDescription: TestProductDescription,
            isActive: TestIsActive,
            sale: TestSale,
            providerId: _testProviderId,
            brandId: _testBrandId,
            categoryId: _testCategoryId).Value;
    }
    
    public static Product GetTestProduct(bool isActive)
    {
        ResetGuids();
        
        return Product.Create(
            id: _testId,
            productName: TestName,
            quantity: TestQuantity,
            fullPrice: TestPrice,
            mainImage: TestMainImage,
            images: _testProductImages,
            productDescription: TestProductDescription,
            isActive: isActive,
            sale: TestSale,
            providerId: _testProviderId,
            brandId: _testBrandId,
            categoryId: _testCategoryId).Value;
    }
    
    public static Product GetTestProduct(IReadOnlyCollection<ProductImage> productImages)
    {
        ResetGuids();
        
        return Product.Create(
            id: _testId,
            productName: TestName,
            quantity: TestQuantity,
            fullPrice: TestPrice,
            mainImage: TestMainImage,
            images: productImages,
            productDescription: TestProductDescription,
            isActive: TestIsActive,
            sale: TestSale,
            providerId: _testProviderId,
            brandId: _testBrandId,
            categoryId: _testCategoryId).Value;
    }

    public static Product GetTestProduct(Guid id)
    {
         ResetGuids();
         
        return Product.Create(
            id: id, 
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
            categoryId: _testCategoryId).Value;
    }
}