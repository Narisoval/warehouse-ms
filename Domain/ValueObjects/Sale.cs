using Domain.Primitives;
using ValueOf;

namespace Domain.ValueObjects;

public class Sale : NumericValueObjectInRange<decimal,Sale>
{
    internal override Range<decimal> Range => Range<decimal>.Create(0M, 100M);
}