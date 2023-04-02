using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class ProductDescription : LengthRestrictedValueObject<ProductDescription>
{
    internal override Range<int> LengthRange { get; } = Range<int>.Create(30, 800); 
}