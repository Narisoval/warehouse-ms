using Domain.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Provider : Entity
{
    public string CompanyName { get; private set; }
    public string PhoneNumber { get; private set; }
    public Email Email { get; private set; }
    public Provider(Guid id, string companyName, string phoneNumber, Email email) : base(id)
    {
        CompanyName = companyName;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void ChangePhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public void ChangeEmail(Email email)
    {
        Email = email;
    }
}