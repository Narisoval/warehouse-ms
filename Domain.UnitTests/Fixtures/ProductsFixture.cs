using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductsFixture
{
    private static readonly ProductName TestName = ProductName.From("Kingston 3200 TB hard drive");
    private static readonly Quantity TestQuantity = Quantity.From(300);
    private static readonly Price TestPrice = Price.From(500.99M);
    private static readonly ProductDescription TestProductDescription = ProductDescription.From(new string('1', 50));
    private static readonly bool TestIsActive = true;
    private static readonly Sale TestSale = Sale.From(0);
    public static Product GetTestProduct()
    {
        Guid testId = Guid.NewGuid();
        IList<ProductImage> testProductImages = ProductImagesFixture.GetTestProductImages();
        Brand testBrand = BrandsFixture.GetTestBrand();
        Category testCategory = CategoriesFixture.GetTestCategory();
        Provider testProvider = ProvidersFixture.GetTestProvider();
        
        return Product.Create(testId,
            TestName,
            TestQuantity,
            TestPrice,
            testProductImages,
            TestProductDescription,
            TestIsActive,
            TestSale,
            testProvider,
            testBrand,
            testCategory);
    }
    
    public static Product GetTestProduct(int intQuantity)
    {
        Guid testId = Guid.NewGuid();
        IList<ProductImage> testProductImages = ProductImagesFixture.GetTestProductImages();
        Brand testBrand = BrandsFixture.GetTestBrand();
        Category testCategory = CategoriesFixture.GetTestCategory();
        Provider testProvider = ProvidersFixture.GetTestProvider();
        
        return Product.Create(testId,
            TestName,
            Quantity.From(intQuantity),
            TestPrice,
            testProductImages,
            TestProductDescription,
            TestIsActive,
            TestSale,
            testProvider,
            testBrand,
            testCategory);
    }
    
    public static Product GetTestProduct(bool isActiveParam)
    {
        Guid testId = Guid.NewGuid();
        IList<ProductImage> testProductImages = ProductImagesFixture.GetTestProductImages();
        Brand testBrand = BrandsFixture.GetTestBrand();
        Category testCategory = CategoriesFixture.GetTestCategory();
        Provider testProvider = ProvidersFixture.GetTestProvider();
        
        return Product.Create(testId,
            TestName,
            TestQuantity,
            TestPrice,
            testProductImages,
            TestProductDescription,
            isActiveParam,
            TestSale,
            testProvider,
            testBrand,
            testCategory);
    }
    
    public static Product GetTestProduct(IList<ProductImage> productImages)
    {
        Guid testId = Guid.NewGuid();
        Brand testBrand = BrandsFixture.GetTestBrand();
        Category testCategory = CategoriesFixture.GetTestCategory();
        Provider testProvider = ProvidersFixture.GetTestProvider();
        
        return Product.Create(testId,
            TestName,
            TestQuantity,
            TestPrice,
            productImages,
            TestProductDescription,
            TestIsActive,
            TestSale,
            testProvider,
            testBrand,
            testCategory);
    }

    public static Product GetTestProduct(Guid id)
    {
         IList<ProductImage> testProductImages = ProductImagesFixture.GetTestProductImages();
         Brand testBrand = BrandsFixture.GetTestBrand();
         Category testCategory = CategoriesFixture.GetTestCategory();
         Provider testProvider = ProvidersFixture.GetTestProvider();
         
         return Product.Create(id,
             TestName,
             TestQuantity,
             TestPrice,
             testProductImages,
             TestProductDescription,
             TestIsActive,
             TestSale,
             testProvider,
             testBrand,
             testCategory);       
    }
}