using System.Reflection;
using ValueOf;

namespace Domain.Primitives;

public abstract class StringValueObjectWithLengthRestrictions<T> : ValueOf<string,T> 
    where T : StringValueObjectWithLengthRestrictions<T>, new()
{
    internal abstract Range<int> LengthRange { get; }

    protected override void Validate()
    {
        CheckStringForNull();
        CheckStringLength();
    }

    private void CheckStringForNull()
    {
        if (Value is null)
        {
            throw new ArgumentNullException(nameof(Value));
        }
    }

    private void CheckStringLength()
    {
        if (Value.Length < LengthRange.Min || Value.Length > LengthRange.Max)
        {
            throw new ArgumentOutOfRangeException
                (nameof(base.Value),$"{typeof(T).GetTypeInfo().Name} length should be between {LengthRange.Min} and {LengthRange.Max}");
        }
    }
}