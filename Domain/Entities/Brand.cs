using Domain.Errors;
using Domain.Primitives;
using Domain.ValueObjects;
using FluentResults;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Domain.Entities;

public class Brand : Entity
{
    public BrandName Name { get; private set; }
    public Image Image { get; private set; }
    public BrandDescription Description { get; private set; }

    public IReadOnlyCollection<Product>? Products { get; private set; }


    public static Result<Brand> Create(Guid id, BrandName brandName, Image brandImage, BrandDescription brandDescription)
    {
        Result<Brand> result = new Result<Brand>();
        
        if (id == Guid.Empty)
            result.WithError(new EmptyGuidError(nameof(Brand)));
            
        if (result.IsFailed)
            return result;
        
        return new Brand(
            id: id,
            brandName: brandName,
            brandImage: brandImage, 
            brandDescription: brandDescription);
    }

    public static Result<Brand> Create(BrandName brandName, Image brandImage, BrandDescription brandDescription)
    {
        Guid id = Guid.NewGuid();
        return Create(id,brandName,brandImage,brandDescription);
    }
    
    private Brand(Guid id, BrandName brandName, Image brandImage, BrandDescription brandDescription) : base(id)
    {
        Name = brandName;
        Image = brandImage;
        Description = brandDescription;
    }

    //For EF
    #pragma warning disable CS8618
    private Brand() 
    {
        ;
    }
}
