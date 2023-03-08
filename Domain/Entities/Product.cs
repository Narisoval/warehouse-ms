using System.Net.Mime;
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
    
    public Image MainImage { get; private set; }
    
    public IReadOnlyCollection<ProductImage>? Images { get; private set; }

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
        Image? mainImage,
        IReadOnlyCollection<ProductImage>? images, 
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

        result.WithErrors(CheckProductImages(mainImage, images).Errors);

        if (productDescription == null)
            result.WithError(new NullArgumentError(nameof(Description)));
        
        if(sale == null)
            result.WithError(new NullArgumentError(nameof(Description)));

        if (result.IsFailed)
            return result;
        
        return new Product(id,productName!,quantity!,fullPrice!,mainImage!,images!,productDescription!,
            isActive,sale!,providerId,brandId,categoryId);
    }
    
    public static Result<Product> Create(
        ProductName? productName, 
        Quantity? quantity, 
        Price? fullPrice, 
        Image? mainImage,
        IReadOnlyCollection<ProductImage>? images, 
        ProductDescription? productDescription, 
        bool isActive, 
        Sale? sale, 
        Guid providerId, 
        Guid brandId, 
        Guid categoryId)
    {
        Guid id = Guid.NewGuid();
        return Create(id,productName,quantity,fullPrice,mainImage,images,productDescription,
            isActive,sale,providerId,brandId,categoryId);
    }
    
    private Product(
        Guid id,
        ProductName productName,
        Quantity quantity,
        Price fullPrice,
        Image mainImage,
        IReadOnlyCollection<ProductImage> images,
        ProductDescription productDescription,
        bool isActive,
        Sale sale,
        Guid providerId,
        Guid brandId,
        Guid categoryId) : this(id,productName,quantity,fullPrice,mainImage,images,productDescription,isActive,sale)
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
        Image mainImage,
        IReadOnlyCollection<ProductImage> images,
        ProductDescription productDescription,
        bool isActive,
        Sale sale) : base(id)
    {
        Name = productName;
        Quantity = quantity;
        FullPrice = fullPrice;
        MainImage = mainImage;
        Images = images;
        Description = productDescription;
        IsActive = isActive;
        Sale = sale;
    }
    
    private static Result CheckProductImages(Image? mainImage, IReadOnlyCollection<ProductImage>? images)
    {
        Result result = new Result();     
        
        if (mainImage == null)
            result.WithError(new NullArgumentError(nameof(MainImage)));

        if (images == null)
            result.WithError(new NullArgumentError(nameof(Images)));
        else
        {
            if (images.Any(image => image.Image == mainImage))
                result.WithError($"{nameof(Images)} can't contain {nameof(MainImage)}");
        }
        
        return result;
    }

    public void SetProductImages(IReadOnlyCollection<ProductImage>? images)
    {
        if (images != null)
        {
            foreach (var productImage in images)
            {
                productImage.ProductId = Id;
                productImage.Product = this;
            }
        }
        
        Images = images;
    }
    
    //For EF 
    private Product()
    {
    }
}