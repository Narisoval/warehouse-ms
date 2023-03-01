using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class BrandName : LengthRestrictedValueObject<BrandName>,IRanged<uint>
{
    Range<uint> IRanged<uint>.Range { get; } = Range<uint>.Create(2, 30);
}