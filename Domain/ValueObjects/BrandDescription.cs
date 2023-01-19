using Domain.Exceptions;
using Domain.Primitives;

namespace Domain.ValueObjects;

public class BrandDescription : ValueObject
{
    private const int MinimumLength = 20;
    private const int MaximumLength = 800;
    
    public string Value { get; }
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    private BrandDescription(string description)
    {
        Value = description;
    }

    private static BrandDescription Create(string description)
    {
        if (description.Length < 20 || description.Length > 800)
        {
            throw new IncorrectLengthException(MinimumLength, 
                MaximumLength, "Brand Description");
        }

        return new BrandDescription(description);
    }
}