using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class BrandName : LengthRestrictedValueObject<BrandName>
{
    internal override Range<int> LengthRange { get; } = Range<int>.Create(2, 30);
}