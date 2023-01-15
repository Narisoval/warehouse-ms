using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Product : Entity
{
    public ProductName ProductName { get; private set; }
    public Quantity Quantity { get; private set; }
    public Price Price { get; private set; }
    public string ImageUri { get; private set; }
    public Description Description { get; private set; }
    
    public Guid ProviderId  { get; private set; }
    public Provider? Provider { get; private set; }

    public Guid BrandId { get; private set; }
    public Brand? Brand { get; private set; }

    public void DecreaseQuantityBy(int amount)
    {
        this.Quantity = Quantity.Create(this.Quantity.Value - amount);
    }

    public void IncreaseQuantityBy(int amount)
    {
        this.Quantity = Quantity.Create(this.Quantity.Value + amount);
    }

    public Product(Guid id, ProductName productName, Quantity quantity, Price price, string imageUri, Description description, Provider provider, Brand brand) : base(id)
    {
        ProductName = productName;
        Quantity = quantity;
        Price = price;
        ImageUri = imageUri;
        Description = description;
        Provider = provider;
        ProviderId = provider.Id;
        Brand = brand;
        BrandId = brand.Id;
    }
}