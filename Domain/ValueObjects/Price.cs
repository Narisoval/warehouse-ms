using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Price : NumericValueObjectInRange<decimal,Price>
{
    internal override Range<decimal> Range => Range<decimal>.Create(0.5M, 1_000_000M);
}