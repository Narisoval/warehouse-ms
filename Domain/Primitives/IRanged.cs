namespace Domain.Primitives;

public interface IRanged<T> where T : struct, IComparable
{
    internal Range<T> Range { get; }
}