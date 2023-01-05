using ValueOf;

namespace Domain.ValueObjects;

public class ProductName : ValueOf<string,ProductName>
{
    private const int MinimumNameSize = 10;
    private const int MaximumNameSize = 70;
    protected override void Validate()
    {
        if (Value is null)
        {
            throw new ArgumentNullException($"Value","Name of the product cannot be null");
        }

        if (Value.Length is <= MinimumNameSize or >= MaximumNameSize)
        {
            throw new ArgumentOutOfRangeException($"Value", Value,
        "Name of the product is incorrect length. The length should be" +
                $"between {MinimumNameSize} and {MinimumNameSize}.");
        }
    }
    
}