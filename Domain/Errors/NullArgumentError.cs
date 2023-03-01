using FluentResults;

namespace Domain.Errors;

public class NullArgumentError : Error
{
    public NullArgumentError(string name)
        : base($"{name} can't be null")

    {

    }

    public NullArgumentError() : base("This value can't be null")
    {
    }
}