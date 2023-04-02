using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Price : RangedValueObject<Price,decimal>
{
    internal override Range<decimal> Range { get; } = Range<decimal>.Create(0.5M, 1_000_000M);
}