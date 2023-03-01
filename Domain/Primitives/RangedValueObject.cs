using Domain.Errors;
using FluentResults;

namespace Domain.Primitives;
public class RangedValueObject<TThis,TValue> : ValueObject 
    where TValue : struct, IComparable 
    where TThis : RangedValueObject<TThis,TValue>, IRanged<TValue>, new()
{
    public TValue Value { get; private set; }
    
    protected RangedValueObject()
    {
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value; 
    }
    
    public static Result<TThis> From(TValue value)
    {
        var valueObject = new TThis();
        var result = valueObject.Validate(value,valueObject);
        
        if (!result.IsSuccess) return result;
        
        valueObject.Value = value;
        return valueObject;
    }

    private Result<TThis> Validate(TValue value, IRanged<TValue> ranged)
    {
        if (!ranged.Range.InRange(value))
            return new Result<TThis>().WithError(
                new OutOfRangeError(ranged.Range.ToString()));
                
        return Result.Ok();
    }
}
