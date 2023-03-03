using FluentResults;

namespace Domain.Errors;

public class OutOfRangeError : Error
{
    public OutOfRangeError(string name, string range) : base($"{name}'s value must be in the range of: {range}")
    {
        ;
    }
}