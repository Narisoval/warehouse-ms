using FluentResults;

namespace Domain.Errors;

public class EmptyGuidError : Error
{
    public EmptyGuidError(string entityName) 
        : base($"{entityName} can't have an empty guid as id.")
    {
        
    }
}