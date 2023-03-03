using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class ProductName : LengthRestrictedValueObject<ProductName>, IRanged<uint>
{
    Range<uint> IRanged<uint>.Range { get; } = Range<uint>.Create(10, 70);
}