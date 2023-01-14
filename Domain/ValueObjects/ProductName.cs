using Domain.Exceptions;
using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class ProductName : ValueObject 
{
    public string Value { get; }
    private const int MinimumNameSize = 10;
    private const int MaximumNameSize = 70;

    public ProductName(string value)
    {
        Value = value;
    }

    public static ProductName Create(string productName)
    {
        if (productName is null)
        {
            throw new ArgumentNullException($"Value","Name of the product cannot be null");
        }

        if (productName.Length is <= MinimumNameSize or >= MaximumNameSize)
        {
            throw new IncorrectLengthException(MinimumNameSize, MaximumNameSize, "Product name");
        }

        return new ProductName(productName);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}