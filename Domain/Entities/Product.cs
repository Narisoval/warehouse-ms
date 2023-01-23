using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Product : Entity
{
    public ProductName ProductName { get; private set; }
    public Quantity Quantity { get; private set; }
    public Price FullPrice { get; private set; }
    //TODO make a list of them and a new entity ProductImage
    public Image Image { get; private set; }
    public ProductDescription ProductDescription { get; private set; }
    public bool IsActive { get; private set; }
    public Sale Sale { get; private set; }
    public Guid ProviderId  { get; private set; }
    public Provider Provider { get; private set; }

    public Guid BrandId { get; private set; }
    public Brand Brand { get; private set; }

    public Product(Guid id, ProductName productName, Quantity quantity, Price fullPrice, Image image, 
        ProductDescription productDescription, bool isActive, Sale sale, 
        Provider provider, Brand brand) : base(id)
    {
        ProductName = productName;
        Quantity = quantity;
        FullPrice = fullPrice;
        Image = image;
        ProductDescription = productDescription;
        IsActive = isActive;
        Sale = sale;
        Provider = provider;
        ProviderId = Provider.Id;
        Brand = brand;
        BrandId = Brand.Id;
    }

    public void DecreaseQuantityBy(int amount)
    {
        this.Quantity = Quantity.From(this.Quantity.Value - amount);
    }

    public void IncreaseQuantityBy(int amount)
    {
        this.Quantity = Quantity.From(this.Quantity.Value + amount);
    }

}