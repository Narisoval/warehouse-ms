using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;

namespace Domain.UnitTests.Entities;

public class ProviderTests
{
    [Fact]
    public void Should_CreateProvider_When_AllPropertiesAreValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var companyName = "Acme Corp";
        var phoneNumber = "555-555-5555";
        var email = Email.From("test@example.com");

        // Act
        var provider = Provider.Create(id, companyName, phoneNumber, email);

        // Assert
        Assert.Equal(id, provider.Id);
        Assert.Equal(companyName, provider.CompanyName);
        Assert.Equal(phoneNumber, provider.PhoneNumber);
        Assert.Equal(email, provider.Email);
    }

    [Fact]
    public void Should_ChangePhoneNumber_When_NewPhoneNumberIsValid()
    {
        // Arrange
        var provider = ProviderFixture.CreateProvider();
        var newPhoneNumber = "666-666-6666";

        // Act
        provider.ChangePhoneNumber(newPhoneNumber);

        // Assert
        Assert.Equal(newPhoneNumber, provider.PhoneNumber);
    }

    [Fact]
    public void Should_ChangeEmail_When_NewEmailIsValid()
    {
        // Arrange
        var provider = ProviderFixture.CreateProvider();
        var newEmail = Email.From("exampleee@example.com");

        // Act
        provider.ChangeEmail(newEmail);

        // Assert
        Assert.Equal(newEmail, provider.Email);
    }
}