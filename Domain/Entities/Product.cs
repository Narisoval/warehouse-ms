using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Product : Entity
{
    public ProductName ProductName { get; set; }
    public Quantity Quantity { get; set; }
    public Price Price { get; set; }
    public string ImageUri { get; set; }
    public Description Description { get; set; }
    
    public Guid ProviderId  { get; set; }
    public Provider? Provider { get; set; }

    public Guid BrandId { get; set; }
    public Brand? Brand { get; set; }

    public void DecreaseQuantityBy(int amount)
    {
        this.Quantity = Quantity.Create(this.Quantity.Value - amount);
    }

    public void IncreaseQuantityBy(int amount)
    {
        this.Quantity = Quantity.Create(this.Quantity.Value + amount);
    }

    public static Entity Create(Quantity quantity, string imageUri, Description description, Guid brandId, Guid providerId)
    {
        return new Product(Guid.NewGuid(),quantity,imageUri,description,brandId,providerId);
    }

    private Product(Guid id, Quantity quantity, 
        string imageUri, Description description, 
        Guid brandId, Guid providerId) : base(id)
    {
        Id = id;
        this.Quantity = quantity;
        ImageUri = imageUri;
        this.Description = description;
        BrandId = brandId;
        ProviderId = providerId;

    }
}