using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductsFixture
{
    private static List<Brand>? _brands;
    public static Product GetProductWithFixedQuantity(int quantity)
    {
        _brands = BrandsFixture.GetBrands().ToList();
        return Product.Create(Guid.NewGuid(),
            ProductName.From("A great product"),
            Quantity.From(quantity),
            Price.From(600),
            new List<ProductImage>() { ProductImage.Create(Guid.NewGuid(), Image.From("https://cat.png")) },
            ProductDescription.From(new string('a',50)),
            true,
            Sale.From(0),
            new Provider(Guid.NewGuid(), "Nike inc.","+380689438934",Email.From("example@ex.com")),
            _brands[0]);
    }
}