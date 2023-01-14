using Domain.Exceptions;
using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Description : ValueObject
{
    private const int MinimumNameSize = 30;
    private const int MaximumNameSize = 1200;
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }
    
    public static Description Create(string description)
    {
        if (description is null)
        {
            throw new ArgumentNullException($"Value","Description of the product cannot be null");
        }

        if (description.Length is < MinimumNameSize or > MaximumNameSize)
        {
            throw new IncorrectLengthException(MinimumNameSize, MaximumNameSize, "Description");
        }

        return new Description(description);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}