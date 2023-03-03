using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Sale : RangedValueObject<Sale,decimal>, IRanged<decimal>
{
    Range<decimal> IRanged<decimal>.Range => Range<decimal>.Create(0M, 100);
}