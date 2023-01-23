using Domain.Validation;
using ValueOf;

namespace Domain.ValueObjects;

public sealed class Price : ValueOf<decimal, Price>
{
    private static readonly Range<decimal> PriceRange = Range<decimal>.Create(0.5M, 1_000_000M);

    protected override void Validate()
    {
        if (Value < PriceRange.Min || Value > PriceRange.Max)
        {
            throw new ArgumentOutOfRangeException
            (nameof(Value),
                Value,$"Price of a product cannot be less than {PriceRange.Min} or more than {PriceRange.Max}");
        }
    }

    public static Range<decimal> GetPriceRange()
    {
        return Range<decimal>.Create(PriceRange.Min, PriceRange.Max);
    }

}