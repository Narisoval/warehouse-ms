using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class BrandDescription : LengthRestrictedValueObject<BrandDescription>
{
    internal override Range<int> LengthRange { get; } = Range<int>.Create(10, 800);
}
    