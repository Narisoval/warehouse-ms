namespace Domain.Primitives;

internal class Range<T> where T : struct, IComparable
{
    public T Min { get; }
    public T Max { get; }

    private Range(T min, T max)
    {
        if (min.CompareTo(max) <= 0)
        {
            Min = min;
            Max = max;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(min), "Min cannot be greater than max");
        }
    }

    public static Range<T> Create(T min, T max)
    {
        return new Range<T>(min, max);
    }
}