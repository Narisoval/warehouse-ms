using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class BrandDescription : LengthRestrictedValueObject<BrandDescription>,IRanged<uint>
{
    Range<uint> IRanged<uint>.Range { get; } = Range<uint>.Create(10, 800);
}
    