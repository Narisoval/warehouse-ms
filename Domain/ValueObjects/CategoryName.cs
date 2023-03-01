using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class CategoryName : LengthRestrictedValueObject<CategoryName>, IRanged<uint>
{
    Range<uint> IRanged<uint>.Range { get; } = Range<uint>.Create(2, 30);
}