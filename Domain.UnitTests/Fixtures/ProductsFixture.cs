using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductsFixture
{
    private static readonly Guid TestId = Guid.NewGuid();
    private static readonly ProductName TestName = ProductName.From("Kingston 3200 TB hard drive");
    private static readonly Quantity TestQuantity = Quantity.From(300);
    private static readonly Price TestPrice = Price.From(500.99M);
    private static readonly IList<ProductImage> TestProductImages = ProductImagesFixture.GetTestProductImages();
    private static readonly ProductDescription TestProductDescription = ProductDescription.From(new string('1', 50));
    private static readonly bool TestIsActive = true;
    private static readonly Sale TestSale = Sale.From(0);
    private static readonly Provider TestProvider = ProvidersFixture.GetTestProvider();
    private static readonly Brand TestBrand = BrandsFixture.GetTestBrand();
    private static readonly Category TestCategory = CategoriesFixture.GetTestCategory();
    public static Product GetTestProduct()
    {
        return Product.Create(TestId,
            TestName,
            TestQuantity,
            TestPrice,
            TestProductImages,
            TestProductDescription,
            TestIsActive,
            TestSale,
            TestProvider,
            TestBrand,
            TestCategory);
    }
    
    public static Product GetTestProduct(int intQuantity)
    {
        return Product.Create(TestId,
            TestName,
            Quantity.From(intQuantity),
            TestPrice,
            TestProductImages,
            TestProductDescription,
            TestIsActive,
            TestSale,
            TestProvider,
            TestBrand,
            TestCategory);
    }
    
    public static Product GetTestProduct(bool isActiveParam)
    {
        return Product.Create(TestId,
            TestName,
            TestQuantity,
            TestPrice,
            TestProductImages,
            TestProductDescription,
            isActiveParam,
            TestSale,
            TestProvider,
            TestBrand,
            TestCategory);
    }
    
    public static Product GetTestProduct(IList<ProductImage> productImages)
    {
        return Product.Create(TestId,
            TestName,
            TestQuantity,
            TestPrice,
            productImages,
            TestProductDescription,
            TestIsActive,
            TestSale,
            TestProvider,
            TestBrand,
            TestCategory);
    }
}