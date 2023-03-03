using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Price : RangedValueObject<Price,decimal>, IRanged<decimal>
{
    Range<decimal> IRanged<decimal>.Range => Range<decimal>.Create(0.5M, 1_000_000M);
}