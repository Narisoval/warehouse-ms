using Domain.Validation;
using ValueOf;

namespace Domain.ValueObjects;

public sealed class BrandDescription : ValueOf<string,BrandDescription>
{
    private static readonly Range<int> LengthRange = Range<int>.Create(20, 800);
 
    protected override void Validate()
    {
        StringLengthValidator.ValidateStringLength(Value,LengthRange);
    }

    public static Range<int> GetRange()
    {
        return Range<int>.Create(LengthRange.Min, LengthRange.Max);
    }
    
}