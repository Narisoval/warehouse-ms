using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProviderFixture
{
    public static Provider CreateProvider()
    {
        var id = Guid.NewGuid();
        var companyName = "Acme Corp";
        var phoneNumber = "555-555-5555";
        var email = Email.From("test@example.com");

        return Provider.Create(id, companyName, phoneNumber, email);
    }
}