using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class ProductName : StringValueObjectWithLengthRestrictions<ProductName>
{
    internal override Range<int> LengthRange => Range<int>.Create(10, 70);
}