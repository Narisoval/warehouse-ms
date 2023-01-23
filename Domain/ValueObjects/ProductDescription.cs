using Domain.Validation;
using ValueOf;

namespace Domain.ValueObjects;

public sealed class ProductDescription : ValueOf<string,ProductDescription>
{
    private static readonly Range<int> LengthRange = Range<int>.Create(30, 1200);
    protected override void Validate()
    {
        StringLengthValidator.ValidateStringLength(Value,LengthRange);
    }

    public static Range<int> GetRange()
    {
        return Range<int>.Create(LengthRange.Min, LengthRange.Max);
    }
}