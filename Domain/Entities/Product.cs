using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Product : Entity
{
    public ProductName ProductName { get; private set; }
    public Quantity Quantity { get; private set; }
    public Price FullPrice { get; private set; }
    public IList<ProductImage> Images { get; private set; }
    public ProductDescription ProductDescription { get; private set; }
    public bool IsActive { get; private set; }
    public Sale Sale { get; private set; }
    
    public Provider Provider { get; private set; }
    public Guid ProviderId  { get; private set; }
    
    public Brand Brand { get; private set; }
    public Guid BrandId { get; private set; }

    private Product(
        Guid id, 
        ProductName productName, 
        Quantity quantity, 
        Price fullPrice, 
        IList<ProductImage> productImages, 
        ProductDescription productDescription, 
        bool isActive, 
        Sale sale, 
        Provider provider, 
        Brand brand) : base(id)
    {
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        FullPrice = fullPrice ?? throw new ArgumentNullException(nameof(fullPrice));
        Images = productImages ?? throw new ArgumentException(nameof(productImages));
        ProductDescription = productDescription ?? throw new ArgumentException(nameof(productDescription));
        IsActive = isActive;
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        ProviderId = Provider.Id;
        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        BrandId = Brand.Id;
    }
    
    public static Product Create(Guid id, 
        ProductName productName, 
        Quantity quantity, 
        Price fullPrice, 
        IList<ProductImage> productImages, 
        ProductDescription productDescription, 
        bool isActive, 
        Sale sale, 
        Provider provider, 
        Brand brand)
    {   
        return new Product(id, productName, quantity, fullPrice, productImages, productDescription, isActive, sale, provider, brand);
    }
    
    public void ChangeName(ProductName? productName)
    {
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
    }

    public void DecreaseQuantityBy(int amount)
    {
        Quantity = Quantity.From(this.Quantity.Value - amount);
    }

    public void IncreaseQuantityBy(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount),"Can't increase quantity by negative amount");
        Quantity = Quantity.From(Quantity.Value + amount);
    }

    public void ChangeFullPrice(Price? fullPrice)
    {
        FullPrice = fullPrice ?? throw new ArgumentNullException(nameof(fullPrice));
    }

    public void ChangeImages(IList<ProductImage>? images)
    {
        Images = images ?? throw new ArgumentNullException(nameof(images));
    }

    public void ChangeDescription(ProductDescription? productDescription)
    {
        ProductDescription = productDescription ?? throw new ArgumentNullException(nameof(productDescription));
    }

    public void EnableProduct()
    {
        IsActive = true;
    }
    
    public void DisableProduct()
    {
        IsActive = false;
    }

    public void ChangeSale(Sale? sale)
    {
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
    }
    
    public void ChangeBrand(Brand? brand)
    {
        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        BrandId = brand.Id;
    }

    public void ChangeProvider(Provider? provider)
    {
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        ProviderId = provider.Id;
    }
 
}