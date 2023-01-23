using ValueOf;
using Domain.Validation;

namespace Domain.ValueObjects;

public sealed class ProductName : ValueOf<string,ProductName>
{
    private static readonly Range<int> LengthRange = Range<int>.Create(10, 70);

    protected override void Validate()
    {
        StringLengthValidator.ValidateStringLength(Value,LengthRange);
    }

    public static Range<int> GetRange()
    {
        return Range<int>.Create(LengthRange.Min, LengthRange.Max);
    }
}