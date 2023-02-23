using Domain.Errors;
using Domain.Primitives;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Entities;

public class Provider : Entity
{
    public CompanyName CompanyName { get; set; }
    public string PhoneNumber { get; set; }
    public Email Email { get; set; }

    public IReadOnlyCollection<Product>? Products { get; set; }
    private Provider(Guid id, CompanyName? companyName, string? phoneNumber, Email? email) : base(id)
    {
        CompanyName = companyName ?? throw new ArgumentNullException(nameof(companyName)) ;
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public static Result<Provider> Create(Guid id, CompanyName? companyName, string? phoneNumber, Email? email)
    {
        if (id == Guid.Empty)
        {
            return new Result<Provider>()
                .WithError(new EmptyGuidError(nameof(Provider)));
        }

        return new Provider(id, companyName, phoneNumber, email);
    }

    public static Provider Create(CompanyName companyName, string phoneNumber, Email email)
    {
        Guid id = Guid.NewGuid();
        return new Provider(id, companyName, phoneNumber, email);
    }
}