using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Product : Entity
{
    public ProductName Name { get; private set; }
    
    public Quantity Quantity { get; private set; }
    
    public Price FullPrice { get; private set; }

    private ProductImages? _productImages;
    public IList<ProductImage>? Images
    {
        get => _productImages?.Value;
        set => _productImages = ProductImages.From(value!);
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

    public static Product Create(
        Guid id, 
        ProductName productName,
        Quantity quantity,
        Price fullPrice,
        ProductImages? productImages,
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
    
    public static Product Create(
        Guid id,
        ProductName productName, 
        Quantity quantity, 
        Price fullPrice, 
        ProductImages? images, 
        ProductDescription productDescription, 
        bool isActive, 
        Sale sale, 
        Guid providerId, 
        Guid productId, 
        Guid categoryId)
    {
        return new Product(id,productName,quantity,fullPrice,images,productDescription,
            isActive,sale,providerId,productId,categoryId);
    }
    
    public static Product Create(
        ProductName productName, 
        Quantity quantity, 
        Price fullPrice, 
        ProductImages? images, 
        ProductDescription productDescription, 
        bool isActive, 
        Sale sale, 
        Guid providerId, 
        Guid brandId, 
        Guid categoryId)
    {
        Guid id = Guid.NewGuid();
        return new Product(id,productName,quantity,fullPrice,images,productDescription,
            isActive,sale,providerId,brandId,categoryId);
    }
    
    
    private Product(
        Guid id,
        ProductName productName,
        Quantity quantity,
        Price fullPrice,
        ProductImages? productImages,
        ProductDescription productDescription,
        bool isActive,
        Sale sale,
        Provider? provider,
        Brand? brand,
        Category? category) : this(id,productName,quantity,fullPrice,productImages,productDescription,isActive,sale)
    {
        ChangeProvider(provider);
        ChangeBrand(brand);
        ChangeCategory(category);
    }
    
    private Product(
        Guid id,
        ProductName productName,
        Quantity quantity,
        Price fullPrice,
        ProductImages? productImages,
        ProductDescription productDescription,
        bool isActive,
        Sale sale,
        Guid providerId,
        Guid brandId,
        Guid categoryId) : this(id,productName,quantity,fullPrice,productImages,productDescription,isActive,sale)
    {
        if (providerId == Guid.Empty)
        {
            throw new ArgumentException("Provider id is invalid");
        }
        
        if (brandId == Guid.Empty)
        {
            throw new ArgumentException("Brand id is invalid");
        }
        
        if (categoryId == Guid.Empty)
        {
            throw new ArgumentException("Category id is invalid");
        }
        
        ProviderId = providerId;
        BrandId = brandId;
        CategoryId = categoryId;
    }
    
    private Product(
        Guid id,
        ProductName productName,
        Quantity quantity,
        Price fullPrice,
        ProductImages? productImages,
        ProductDescription productDescription,
        bool isActive,
        Sale sale) : base(id)
    {
        Name = productName ?? throw new ArgumentNullException(nameof(productName));
        Quantity = quantity ?? throw new ArgumentNullException(nameof(quantity));
        FullPrice = fullPrice ?? throw new ArgumentNullException(nameof(fullPrice));
        _productImages = productImages;
        Description = productDescription ?? throw new ArgumentNullException(nameof(productDescription));
        IsActive = isActive;
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
        
        SetProductImagesNavigationalProperties();
    }
    
    //For EF 
    private Product()
    {
    }

    
    public void ChangeAllImages(IList<ProductImage>? images)
    {
        _productImages = ProductImages.From(images!);
        SetProductImagesNavigationalProperties();
    }
    
    private void ChangeProvider(Provider? provider)
    {
        if (provider == null) return;
        Provider = provider;
        ProviderId = provider.Id;
    }
    
    private void ChangeBrand(Brand? brand)
    {
        if (brand == null) return;
        Brand = brand;
        BrandId = brand.Id;
    }
    
    private void ChangeCategory(Category? category)
    {
        if (category == null) return;
        Category = category;
        CategoryId = category.Id;
    }
    private void SetProductImagesNavigationalProperties()
    {
        if (_productImages == null ||  _productImages.Value == null) return;
        foreach (var image in _productImages?.Value!)
        {
            image.ProductId = Id;
            image.Product = this;
        }
    }
}