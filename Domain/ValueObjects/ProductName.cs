using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class ProductName : LengthRestrictedValueObject<ProductName>
{
    internal override Range<int> LengthRange { get; } = Range<int>.Create(10, 70);
}