using Domain.Errors;
using Domain.Primitives;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Entities;

public class ProductImage : Entity
{
    public Image Image { get; internal set; }

    public bool IsMain { get; set; }
    
    public Product? Product { get; set; }
    
    public Guid ProductId { get; set; }

    private ProductImage(Guid id, Image image, bool isMain) : base(id)
    {
        Image = image;
        IsMain = isMain;
    }

    public static Result<ProductImage> Create(Guid id, Image? image, bool isMain)
    {
        if (image == null)
            return new Result<ProductImage>().WithError(new NullArgumentError(nameof(Image)));
        
        return new ProductImage(id, image,isMain);
    }

    public static Result<ProductImage> Create(Image? image, bool isMain)
    {
        Guid id = Guid.NewGuid();
        return Create(id,image,isMain);
    }
}