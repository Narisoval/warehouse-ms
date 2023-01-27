using Domain.Primitives;

namespace Domain.ValueObjects;

public class CompanyName : StringValueObjectWithLengthRestrictions<CompanyName>
{
    internal override Range<int> LengthRange => Range<int>.Create(2, 50);
}