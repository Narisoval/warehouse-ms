using Domain.Exceptions;
using ValueOf;

namespace Domain.ValueObjects;

public sealed class ProductName : ValueOf<string,ProductName>
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
            throw new IncorrectLengthException(MinimumNameSize, MaximumNameSize, "Product name");
        }
    }
}