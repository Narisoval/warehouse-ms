using Domain.Errors;
using Domain.Primitives;
using FluentResults;

namespace Domain.ValueObjects;

public sealed class BrandName : ValueObject 
{
    public string Value { get; }
    private static Range<int> LengthRange => Range<int>.Create(2, 30);

    private BrandName(string brandDescription)
    {
        Value = brandDescription;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    } 

    public static Result<BrandName> From(string? brandName)
    {
        if (brandName == null)
            return new Result<BrandName>().WithError(new NullArgumentError(nameof(BrandName)));
        
        if (brandName.Length < LengthRange.Min || brandName.Length > LengthRange.Max)
            return new Result<BrandName>()
                .WithError(new IncorrectLengthError(nameof(BrandName),LengthRange.Min,LengthRange.Max));
        

        return new BrandName(brandName);
    }
}