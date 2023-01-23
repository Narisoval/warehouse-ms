using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class ProductDescription : StringValueObjectWithLengthRestrictions<ProductDescription>
{
    internal override Range<int> LengthRange => Range<int>.Create(30, 1200);
}