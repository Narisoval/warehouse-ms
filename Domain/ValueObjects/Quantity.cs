using ValueOf;

namespace Domain.ValueObjects;

public class Quantity : ValueOf<int,Quantity>
{
    protected override void Validate()
    {
        if (Value < 0)
        {
            throw new ArgumentOutOfRangeException
                ($"Value","Quantity of a product cannot be less than 0");
        }
    }
}