using Domain.Errors;
using Domain.Primitives;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Entities;

public class Brand : Entity
{
    public BrandName Name { get; set; }
    public Image Image { get; set; }
    public BrandDescription Description { get; set; }

    public IReadOnlyCollection<Product>? Products { get; set; }

    //for EF
    private Brand()
    {
        
    }
    
    private Brand(Guid id, BrandName brandName, Image brandImage, BrandDescription brandDescription) : base(id)
    {
        Name = brandName;
        Image = brandImage;
        Description = brandDescription;
    }

    public static Result<Brand> Create(Guid id, BrandName? brandName, Image? brandImage, BrandDescription? brandDescription)
    {
        Result<Brand> result = new Result<Brand>();
        
        if (id == Guid.Empty)
            result.WithError(new EmptyGuidError(nameof(Brand)));
        
        if (brandName == null)
            result.WithError(new NullArgumentError(nameof(Name)));
        
        if (brandImage == null)
            result.WithError(new NullArgumentError(nameof(Image)));
        
        if(brandDescription == null)
            result.WithError(new NullArgumentError(nameof(brandDescription)));

        if (result.IsFailed)
            return result;
        
        return new Brand(
            id: id,
            brandName: brandName!,
            brandImage: brandImage!, 
            brandDescription: brandDescription!);
    }

    public static Result<Brand> Create(BrandName? brandName, Image? brandImage, BrandDescription? brandDescription)
    {
        Guid id = Guid.NewGuid();
        return Create(id,brandName,brandImage,brandDescription);
    }
}