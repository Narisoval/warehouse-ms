using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Provider : Entity
{
    public string CompanyName { get; private set; }
    public string PhoneNumber { get; private set; }
    public Email Email { get; private set; }

    private Provider(Guid id, string? companyName, string? phoneNumber, Email? email) : base(id)
    {
        CompanyName = companyName ?? throw new ArgumentNullException(nameof(companyName)) ;
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public static Provider Create(Guid id, string companyName, string phoneNumber, Email email)
    {
        return new Provider(id, companyName, phoneNumber, email);
    }

    public void ChangePhoneNumber(string? phoneNumber)
    {
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
    }

    public void ChangeEmail(Email? email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }
}