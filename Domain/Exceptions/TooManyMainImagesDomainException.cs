namespace Domain.Exceptions;

public class TooManyMainImagesDomainException : DomainException
{
    public TooManyMainImagesDomainException(int maxNumberOfImages) :
        base($"There can't be more than {maxNumberOfImages} main images")
    {
    }
}