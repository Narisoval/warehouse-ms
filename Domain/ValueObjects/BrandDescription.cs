using Domain.Errors;
using Domain.Primitives;
using FluentResults;

namespace Domain.ValueObjects;

public sealed class BrandDescription : ValueObject 
{
    private static Range<int> LengthRange => Range<int>.Create(20, 800);
    
    public string Value { get; } 
    
    private BrandDescription(string brandDescription)
    {
        Value = brandDescription;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    } 

    public static Result<BrandDescription> From(string? brandName)
    {
        if (brandName == null)
            return new Result<BrandDescription>().WithError(new NullArgumentError(nameof(BrandDescription)));
        
        if (brandName.Length < LengthRange.Min || brandName.Length > LengthRange.Max)
            return new Result<BrandDescription>()
                .WithError(new IncorrectLengthError(nameof(BrandDescription),LengthRange.Min,LengthRange.Max));
        

        return new BrandDescription(brandName);
    }
    
}