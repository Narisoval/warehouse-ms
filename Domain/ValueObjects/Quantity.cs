
using Domain.Primitives;
using FluentResults;

namespace Domain.ValueObjects;

public sealed class Quantity : ValueObject 
{
    public int Value { get; }

    private Quantity(int quantity)
    {
        Value = quantity;
    }

    public static Result<Quantity> From(int quantity)
    {
        if (quantity < 0)
            return new Result<Quantity>()
                .WithError(new Error("Quantity can't be less than zero"));

        return new Quantity(quantity);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}