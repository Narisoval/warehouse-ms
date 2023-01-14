using Domain.ValueObjects;

namespace Domain.UnitTests.Entities.Product.Fixtures;
using Domain.Entities;
public static class ProductsFixture
{
    public static IEnumerable<Product> GetTestProducts()
    {
        yield return Product.Create(
            Quantity.Create(3), 
    "https://fsdfsdsDF.com", 
            Description.Create("fdjgkldfjgldf"),
        Guid.NewGuid(),
            Guid.NewGuid()
            );

    }

}