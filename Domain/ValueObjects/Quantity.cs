using ValueOf;

namespace Domain.ValueObjects;

public sealed class Quantity : ValueOf<int,Quantity>
{
    protected override void Validate()
    {
        if (Value < 0)
        {
            throw new ArgumentOutOfRangeException
                (nameof(Value),"Quantity of a product cannot be less than 0");
        }
    }

}