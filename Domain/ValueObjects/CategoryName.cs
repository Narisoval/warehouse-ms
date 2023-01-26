using Domain.Primitives;

namespace Domain.ValueObjects;

public class CategoryName : StringValueObjectWithLengthRestrictions<CategoryName>
{
    internal override Range<int> LengthRange => Range<int>.Create(2, 30);
}