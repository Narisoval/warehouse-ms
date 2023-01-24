using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Sale : NumericValueObjectInRange<decimal,Sale>
{
    internal override Range<decimal> Range => Range<decimal>.Create(0M, 100M);
}