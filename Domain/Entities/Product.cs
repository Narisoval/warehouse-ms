using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Product : Entity
{
    public ProductName Name { get; private set; }
    public Quantity Quantity { get; private set; }
    public Price FullPrice { get; private set; }

    private ProductImages _productImages;
    public IList<ProductImage>? Images
    {
        get => _productImages?.Value;
        private set => _productImages = ProductImages.From(value!);
    }

    public ProductDescription Description { get; private set; }
    public bool IsActive { get; private set; }
    public Sale Sale { get; private set; }

    public Provider? Provider { get; private set; }
    public Guid ProviderId { get; private set; }

    public Brand? Brand { get; private set; }
    public Guid BrandId { get; private set; }

    public Category? Category { get; private set; }
    public Guid CategoryId { get; private set; }

    private Product(
        Guid id,
        ProductName productName,
        Quantity quantity,
        Price fullPrice,
        ProductImages productImages,
        ProductDescription productDescription,
        bool isActive,
        Sale sale,
        Provider? provider,
        Brand? brand,
        Category? category) : base(id)
    {
        Name = productName ?? throw new ArgumentNullException(nameof(productName));
        Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        FullPrice = fullPrice ?? throw new ArgumentNullException(nameof(fullPrice));
        _productImages = productImages ?? throw new ArgumentNullException(nameof(productImages));
        Description = productDescription ?? throw new ArgumentNullException(nameof(productDescription));
        IsActive = isActive;
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
       
        ChangeProvider(provider);
        ChangeBrand(brand);
        ChangeCategory(category);
    }

    //For EF 
    private Product()
    {
    }

    public static Product Create(Guid id,
        ProductName productName,
        Quantity quantity,
        Price fullPrice,
        ProductImages productImages,
        ProductDescription productDescription,
        bool isActive,
        Sale sale,
        Provider? provider,
        Brand? brand,
        Category? category)
    {
        return new Product(id, productName, quantity, fullPrice, productImages, productDescription, isActive, sale,
            provider, brand, category);
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
            throw new ArgumentOutOfRangeException(nameof(amount), "Can't increase quantity by negative amount");
        Quantity = Quantity.From(Quantity.Value + amount);
    }

    public void ChangeFullPrice(Price? fullPrice)
    {
        FullPrice = fullPrice ?? throw new ArgumentNullException(nameof(fullPrice));
    }

    public void ChangeAllImages(IList<ProductImage>? images)
    {
        _productImages = ProductImages.From(images!);
    }
    
    public void ChangeMainImage(Image? imageToChangeTo)
    {
        Guid mainImageId = GetMainImage().Id;
        ChangeImage(mainImageId,imageToChangeTo);
    }
    
    public ProductImage GetMainImage()
    {
        ProductImage mainImage = _productImages.Value
            .First(productImage => productImage.IsMain);
        return mainImage;
    }
    
    public void ChangeImage(Guid? imageId, Image? imageToChangeTo)
    {
        if (imageToChangeTo == null)
            throw new ArgumentNullException(nameof(imageToChangeTo));

        if (imageId == null)
            throw new ArgumentNullException(nameof(imageId));

        ProductImage productImageToChange = _productImages.GetProductImageById(imageId.Value);
        productImageToChange.Image = imageToChangeTo;
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
        if (brand != null)
        {
            Brand = brand;
            BrandId = brand.Id;
            return;
        }
        
        Brand = null;
    }

    public void ChangeProvider(Provider? provider)
    {
        if (provider != null)
        {
            Provider = provider; 
            ProviderId = provider.Id;
            return;
        }
        Provider = null;
    }

    public void ChangeCategory(Category? category)
    {
        if (category != null)
        {
            Category = category;
            CategoryId = category.Id;
            return;
        }

        Category = null;
    }
}