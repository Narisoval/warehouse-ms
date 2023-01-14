using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Quantity : ValueObject
{
    public int Value { get; }

    private Quantity(int quantity)
    {
        Value = quantity;
    }
    
    public static Quantity Create(int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException
                ($"Value","Quantity of a product cannot be less than 0");
        }

        return new Quantity(quantity);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}