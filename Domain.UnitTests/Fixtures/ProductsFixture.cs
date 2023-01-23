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
                            ProductName.From("Name of the product"),
                            Quantity.From(quantity), 
                            Price.From(300M), 
                    "https://yes.com/unitTests",
                           ProductDescription.From(new string('*',60)),  
                            new Provider(Guid.NewGuid()),
                            _brands[0]);
    }
}