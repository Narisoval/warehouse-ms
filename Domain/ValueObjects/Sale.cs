using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Sale : RangedValueObject<Sale,decimal>
{
    internal override Range<decimal> Range { get; } = Range<decimal>.Create(0M, 100M);
}