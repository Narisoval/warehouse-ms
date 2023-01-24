using Domain.Primitives;

namespace Domain.ValueObjects;

public class BrandName : StringValueObjectWithLengthRestrictions<BrandName>
{
    internal override Range<int> LengthRange => Range<int>.Create(2, 27);
}