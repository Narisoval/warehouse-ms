using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductsFixture
{
    private static List<Brand>? _brands;
    public static Product GetProductWithFixedQuantity(int quantity)
    {
        _brands = BrandsFixture.GetBrands().ToList();
        return new Product(Guid.NewGuid(),
                            new ProductName("Name of the product"),
                            Quantity.Create(quantity), 
                            Price.Create(300M), 
                    "https://yes.com/unitTests",
                           Description.Create(new string('*',60)),  
                            new Provider(Guid.NewGuid()),
                            _brands[0]);
    }
}