using Domain.ValueObjects;

namespace Domain.Entities;

public class Product
{
    public Guid ProductId { get; set; }
    public ProductName ProductName { get; set; }
    public Quantity Quantity { get; set; }
    public Price Price { get; set; }
    public string? Images { get; set; }
    public Description Description { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public Guid ProviderId  { get; set; }
    public Provider Provider { get; set; }

    public void DecreaseQuantityBy(int amount)
    {
        this.Quantity = Quantity.From(this.Quantity.Value - amount);
    }

    public void IncreaseQuantityBy(int amount)
    {
        this.Quantity = Quantity.From(this.Quantity.Value + amount);
    }
}