using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductsFixture
{
    private static List<Brand>? _brands;
    public static Product GetProductWithFixedQuantity(int quantity)
    {
        _brands = BrandsFixture.GetBrands().ToList();
        return new Product(new Guid(),
            ProductName.From("A great product"),
            Quantity.From(quantity),
            Price.From(600),
            Image.From("https://cat.png"),
            ProductDescription.From(new string('a',50)),
            true,
            Sale.From(0),
            new Provider(Guid.NewGuid()),
            _brands[0]);
    }
}