using Domain.Errors;
using Domain.Primitives;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Entities;

public class ProductImage 
{
    public Image Image { get; private set; }
    
    public Product? Product { get; internal set; }
    
    public Guid ProductId { get; internal set; }

    private ProductImage(Image image) 
    {
        Image = image;
    }

    public static Result<ProductImage> Create(Image? image)
    {
        if (image == null)
            return new Result<ProductImage>().WithError(new NullArgumentError(nameof(Image)));
        
        return new ProductImage(image);
    }
}