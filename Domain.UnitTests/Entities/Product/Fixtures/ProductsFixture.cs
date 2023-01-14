using Domain.ValueObjects;

namespace Domain.UnitTests.Entities.Product.Fixtures;
using Domain.Entities;
public static class ProductsFixture
{
    public static IEnumerable<Product> GetTestProducts()
    {
        yield return new Product(
            Guid.NewGuid(),
            Quantity.Create(3),
            "htrhrth",
            Description.Create(new string('a',35)),
            Guid.NewGuid(),
            Guid.NewGuid(), 
            ProductName.Create("Hard drive ABC"),
            Price.Create(3000.22M));

        
        yield return new Product(
            Guid.NewGuid(),
            Quantity.Create(3),
            "agdsg",
            Description.Create(new string('b',55)),
            Guid.NewGuid(),
            Guid.NewGuid(), 
            ProductName.Create("Soft drive ABC"),
            Price.Create(3000.22M));


    }

}