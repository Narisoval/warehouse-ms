namespace Domain.Validation;
public class Range<T> where T : struct, IComparable
{
    public T Min { get; }
    public T Max { get; }

    private Range(T min, T max)
    {
        if (min.CompareTo(max) < 0)
        {
            Min = min;
            Max = max;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(min), "Min cannot be greater than max");

        }
    }

    public static Range<T> Create(T? min, T? max)
    {
        if (!min.HasValue)
        {
            throw new ArgumentNullException(nameof(min),"Max must not be null.");
        }
        
        if (!max.HasValue)
        {
            throw new ArgumentNullException(nameof(max),"Max and max must not be null.");
        }
        
        return new Range<T>(min.Value, max.Value);
    }
}