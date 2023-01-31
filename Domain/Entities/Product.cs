using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Product : Entity
{
    public ProductName Name { get; private set; }
    public Quantity Quantity { get; private set; }
    public Price FullPrice { get; private set; }
    public IList<ProductImage> Images { get; private set; }
    public ProductDescription Description { get; private set; }
    public bool IsActive { get; private set; }
    public Sale Sale { get; private set; }
    
    public Provider Provider { get; private set; }
    public Guid ProviderId  { get; private set; }
    
    public Brand Brand { get; private set; }
    public Guid BrandId { get; private set; }

    public Category Category { get; private set; }
    public Guid CategoryId { get; private set; }
    
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
        Brand brand, 
        Category category) : base(id)
    {
        Name = productName ?? throw new ArgumentNullException(nameof(productName));
        Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        FullPrice = fullPrice ?? throw new ArgumentNullException(nameof(fullPrice));
        Images = productImages ?? throw new ArgumentException(nameof(productImages));
        Description = productDescription ?? throw new ArgumentException(nameof(productDescription));
        IsActive = isActive;
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
        Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        ProviderId = Provider.Id;
        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        BrandId = Brand.Id;;
        Category = category ?? throw new ArgumentException(nameof(category));
        CategoryId = category.Id;
    }

    //For EF 
    private Product() { }

    public static Product Create(Guid id, 
        ProductName productName, 
        Quantity quantity, 
        Price fullPrice, 
        IList<ProductImage> productImages, 
        ProductDescription productDescription, 
        bool isActive, 
        Sale sale, 
        Provider provider, 
        Brand brand,
        Category category)
    {   
        return new Product(id, productName, quantity, fullPrice, productImages, productDescription, isActive, sale, provider, brand, category);
    }
    
    public void ChangeName(ProductName? productName)
    {
        Name = productName ?? throw new ArgumentNullException(nameof(productName));
    }

    public void DecreaseQuantityBy(int amount)
    {
        Quantity = Quantity.From(Quantity.Value - amount);
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

    public void ChangeImage(Guid? imageId, Image? imageToChangeTo)
    {
        if (imageToChangeTo == null)
            throw new ArgumentNullException(nameof(imageToChangeTo));
        
        if (imageId == null)
            throw new ArgumentNullException(nameof(imageId));

        ProductImage productImageToChange = GetProductImageById(imageId.Value);
        productImageToChange.Image = imageToChangeTo;
    }

    private ProductImage GetProductImageById(Guid id)
    {
        try
        {
            return Images.First(img => img.Id == id);
        }
        catch (InvalidOperationException)
        {
            throw new ArgumentException("Image Id is not found in images of this product");
        }
    }
    

    public void ChangeDescription(ProductDescription? productDescription)
    {
        Description = productDescription ?? throw new ArgumentNullException(nameof(productDescription));
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

    public void ChangeCategory(Category? category)
    {
        Category = category ?? throw new ArgumentNullException(nameof(category));
        CategoryId = category.Id;

    }
}