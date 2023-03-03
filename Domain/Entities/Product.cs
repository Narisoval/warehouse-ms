using Domain.Errors;
using Domain.Primitives;
using Domain.ValueObjects;
using FluentResults;

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
        set => _productImages = ProductImages.From(value).Value;
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

    public static Result<Product> Create(
        Guid id,
        ProductName? productName, 
        Quantity? quantity, 
        Price? fullPrice, 
        ProductImages? images, 
        ProductDescription? productDescription, 
        bool isActive, 
        Sale? sale, 
        Guid providerId, 
        Guid brandId, 
        Guid categoryId)
    {
        var result = new Result<Product>();
        
        if(id == Guid.Empty)
            result.WithError(new EmptyGuidError("Product"));
        
        if (providerId == Guid.Empty)
            result.WithError(new EmptyGuidError("Provider"));
        
        if (brandId == Guid.Empty)
            result.WithError(new EmptyGuidError("Brand"));
        
        if (categoryId == Guid.Empty)
            result.WithError(new EmptyGuidError("Category"));
        
        if (productName == null)
            result.WithError(new NullArgumentError(nameof(Name)));
        
        if (quantity == null)
            result.WithError(new NullArgumentError(nameof(Quantity)));
        
        if (fullPrice == null)
            result.WithError(new NullArgumentError(nameof(FullPrice)));

        if (productDescription == null)
            result.WithError(new NullArgumentError(nameof(Description)));
        
        if(sale == null)
            result.WithError(new NullArgumentError(nameof(Description)));

        if (result.IsFailed)
            return result;
        
        return new Product(id,productName!,quantity!,fullPrice!,images,productDescription!,
            isActive,sale!,providerId,brandId,categoryId);
    }
    
    public static Result<Product> Create(
        ProductName? productName, 
        Quantity? quantity, 
        Price? fullPrice, 
        ProductImages? images, 
        ProductDescription? productDescription, 
        bool isActive, 
        Sale? sale, 
        Guid providerId, 
        Guid brandId, 
        Guid categoryId)
    {
        Guid id = Guid.NewGuid();
        return Create(id,productName,quantity,fullPrice,images,productDescription,
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
        Guid providerId,
        Guid brandId,
        Guid categoryId) : this(id,productName,quantity,fullPrice,productImages,productDescription,isActive,sale)
    {
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
        Name = productName;
        Quantity = quantity;
        FullPrice = fullPrice;
        _productImages = productImages;
        Description = productDescription;
        IsActive = isActive;
        Sale = sale;
        
        SetProductImagesNavigationalProperties();
    }
    

    
    public void ChangeAllImages(IList<ProductImage>? images)
    {
        _productImages = ProductImages.From(images).Value;
        SetProductImagesNavigationalProperties();
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
    
    //For EF 
    private Product()
    {
    }
}