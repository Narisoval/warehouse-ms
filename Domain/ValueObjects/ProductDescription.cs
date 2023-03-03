using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class ProductDescription : LengthRestrictedValueObject<ProductDescription>, IRanged<uint>
{
    Range<uint> IRanged<uint>.Range { get; } = Range<uint>.Create(30, 800);
}