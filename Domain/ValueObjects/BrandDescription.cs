using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class BrandDescription : StringValueObjectWithLengthRestrictions<BrandDescription>
{
    internal override Range<int> LengthRange => Range<int>.Create(20, 800);
}