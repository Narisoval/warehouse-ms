using Domain.Errors;
using Domain.Primitives;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Entities;

public class Provider : Entity
{
    public CompanyName CompanyName { get; private set; }
    
    public PhoneNumber PhoneNumber { get; private set; }

    public Email Email { get; private set; }
    
    
    public IReadOnlyCollection<Product>? Products { get; set; }
    
    public static Result<Provider> Create(CompanyName companyName, PhoneNumber phoneNumber, Email email)
    {
        Guid id = Guid.NewGuid();
        return Create(id, companyName, phoneNumber, email);
    }
    
    public static Result<Provider> Create(Guid id, CompanyName companyName, PhoneNumber phoneNumber, Email email)
    {
        Result<Provider> result = new Result<Provider>();
        
        if (id == Guid.Empty)
            result.WithError(new EmptyGuidError(nameof(Provider)));
        
        if (result.IsFailed)
            return result;
        
        return new Provider(id, companyName, phoneNumber, email);
    }
    
    private Provider(Guid id, CompanyName companyName, PhoneNumber phoneNumber, Email email) : base(id)
    {
        CompanyName = companyName; 
        PhoneNumber = phoneNumber; 
        Email = email;
    }
}