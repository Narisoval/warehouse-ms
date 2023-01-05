using ValueOf;

namespace Domain.ValueObjects;

public class Price : ValueOf<decimal,Price>
{
    private const decimal MinPrice = 0.5M;
    private const decimal MaxPrice = 1_000_000M;
    
    protected override void Validate()
    {
        if (Value is < MinPrice or > MaxPrice)
        {
            throw new ArgumentOutOfRangeException
                ($"Value",
                   Value,$"Price of a product cannot be less than {MinPrice} or more than {MaxPrice}");
        }
    }
}