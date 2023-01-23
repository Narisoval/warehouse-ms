namespace Domain.Validation;

public static class StringLengthValidator
{
    public static void ValidateStringLength(string? value, Range<int> range)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (value.Length < range.Min || value.Length > range.Max)
        {
            throw new ArgumentOutOfRangeException
            (nameof(value),$"Value length should be between {range.Min} and {range.Max}");
        }
    }
}
