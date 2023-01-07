using Domain.Exceptions;
using ValueOf;

namespace Domain.ValueObjects;

public class Description : ValueOf<string,Description>
{
    private const int MinimumNameSize = 30;
    private const int MaximumNameSize = 1200;
    protected override void Validate()
    {
        if (Value is null)
        {
            throw new ArgumentNullException($"Value","Description of the product cannot be null");
        }

        if (Value.Length is < MinimumNameSize or > MaximumNameSize)
        {
            throw new IncorrectLengthException(MinimumNameSize, MaximumNameSize, "Description");
        }
    }
    
}