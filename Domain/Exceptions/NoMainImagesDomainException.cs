namespace Domain.Exceptions;

public class NoMainImagesDomainException : DomainException
{
    public NoMainImagesDomainException() : base("There should be at least one main image")
    {
    }
}