using System.Reflection;
using ValueOf;

namespace Domain.Primitives;

public abstract class NumericValueObjectInRange<TValue,TThis> : ValueOf<TValue,TThis> 
    where TValue : struct, IComparable 
    where TThis : NumericValueObjectInRange<TValue,TThis>, new()
{
    internal abstract Range<TValue> Range { get; }


    protected override void Validate()
    {
        if(Value.CompareTo( Range.Min) < 0 || Value.CompareTo(Range.Max) > 0)
        {
            throw new ArgumentOutOfRangeException
            (nameof(Value),
                Value,$"{typeof(TThis).GetTypeInfo().Name} cannot be less than {Range.Min} or more than {Range.Max}");
            
        }
    }

}