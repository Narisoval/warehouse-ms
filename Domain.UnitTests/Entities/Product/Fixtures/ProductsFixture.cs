using Domain.ValueObjects;

namespace Domain.UnitTests.Entities.Product.Fixtures;
using Domain.Entities;
public static class ProductsFixture
{
    
    public static Product GetProductWithFixedQuantity(int quantity)
    {
        return new Product(Guid.NewGuid(),
                            new ProductName("Name of the product"),
                            Quantity.Create(quantity), 
                            Price.Create(300M), 
                    "https://yes.com/unitTests",
                           Description.Create(new string('*',60)),  
                            new Provider(Guid.NewGuid()),
                            new Brand(Guid.NewGuid()));
    }

}