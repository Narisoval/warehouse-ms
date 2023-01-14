using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Price : ValueObject
{
    public decimal Value { get; }
    
    private const decimal MinPrice = 0.5M;
    private const decimal MaxPrice = 1_000_000M;

    private Price(decimal value)
    {
        Value = value;
    }

    public static Price Create(decimal price)
    {
        if (price is < MinPrice or > MaxPrice)
        {
            throw new ArgumentOutOfRangeException
            ($"Value",
                price,$"Price of a product cannot be less than {MinPrice} or more than {MaxPrice}");
        }
        return new Price(price);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}