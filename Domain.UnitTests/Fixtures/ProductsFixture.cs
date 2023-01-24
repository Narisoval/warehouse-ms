using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProductsFixture
{
    public static Product GetTestProduct()
    {
        var id = Guid.NewGuid();
        var productName = ProductName.From("Hard drive Kingston 2TB");
        var quantity = Quantity.From(300);
        var price = Price.From(300);
        var productImages = new List<ProductImage>
            { ProductImage.Create(Guid.NewGuid(), Image.From("https://cat.png")) };
        var productDescription = ProductDescription.From(new string('a', 60));
        var isActive = true;
        var sale = Sale.From(0);
        var provider = Provider.Create(Guid.NewGuid(), "Nike inc.", "+380689438934", Email.From("example@ex.com"));
        var brand = BrandsFixture.GetTestBrand();
        
        return Product.Create(id,
            productName,
            quantity,
            price,
            productImages,
            productDescription,
            isActive,
            sale,
            provider,
            brand);
    }
    
    public static Product GetTestProduct(int intQuantity)
    {
        var id = Guid.NewGuid();
        var productName = ProductName.From("Hard drive Kingston 2TB");
        var quantity = Quantity.From(intQuantity);
        var price = Price.From(300);
        var productImages = new List<ProductImage>
            { ProductImage.Create(Guid.NewGuid(), Image.From("https://cat.png")) };
        var productDescription = ProductDescription.From(new string('a', 60));
        var isActive = true;
        var sale = Sale.From(0);
        var provider = Provider.Create(Guid.NewGuid(), "Nike inc.", "+380689438934", Email.From("example@ex.com"));
        var brand = BrandsFixture.GetTestBrand();
        
        return Product.Create(id,
            productName,
            quantity,
            price,
            productImages,
            productDescription,
            isActive,
            sale,
            provider,
            brand);
    }

    public static Product GetTestProduct(bool isActiveParam)
    {
        var id = Guid.NewGuid();
        var productName = ProductName.From("Hard drive Kingston 2TB");
        var quantity = Quantity.From(300);
        var price = Price.From(300);
        var productImages = new List<ProductImage>
            { ProductImage.Create(Guid.NewGuid(), Image.From("https://cat.png")) };
        var productDescription = ProductDescription.From(new string('a', 60));
        var isActive = isActiveParam;
        var sale = Sale.From(0);
        var provider = Provider.Create(Guid.NewGuid(), "Nike inc.", "+380689438934", Email.From("example@ex.com"));
        var brand = BrandsFixture.GetTestBrand();
         
        return Product.Create(id,
            productName,
            quantity,
            price,
            productImages,
            productDescription,
            isActive,
            sale,
            provider,
            brand);       
    }
}