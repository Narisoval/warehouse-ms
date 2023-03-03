using Domain.Entities;
using Domain.Errors;
using Domain.Primitives;
using FluentResults;

namespace Domain.ValueObjects;

public sealed class ProductImages : ValueObject
{
    // made internal for tests
    internal const int MaxAmountOfMainImages = 1;

    public IList<ProductImage> Value { get; }

    private ProductImages(IList<ProductImage> value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        return Value.AsEnumerable();
    }
    
    public static Result<ProductImages> From(IList<ProductImage>? images)
    {
        if (images == null)
            return new Result<ProductImages>().WithValue(null!);

        //This check is necessary for retrieving products for DB to work correctly.
        //Because when getting Products from db with ProductImages included,
        //EF initializes an empty list of ProductImage that is going through this validation logic.
        //So IF THIS CHECK WILL HAVE TO BE REMOVED IN THE FUTURE,
        //IT SHOULD BE MOVED TO SOME OTHER PLACE, like setter of Images property in Product.       

        if (images.Count == 0)
            return new Result<ProductImages>().WithValue(new(images));
                
        return ValidateAmountOfMainImages(images);
    }

    private static Result<ProductImages> ValidateAmountOfMainImages(IList<ProductImage> images)
    {
        int amountOfMainImages = images.Count(image => image.IsMain);

        if (amountOfMainImages is 0 or > MaxAmountOfMainImages)
            return new Result<ProductImages>()
                .WithError(new IncorrectAmountOfMainImagesError(MaxAmountOfMainImages));
        
        return new Result<ProductImages>().WithValue(new(images));
    }

}