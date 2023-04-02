using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class CategoryName : LengthRestrictedValueObject<CategoryName>
{
    internal override Range<int> LengthRange { get; } = Range<int>.Create(2, 30);
}