using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using FluentResults;

namespace Domain.UnitTests.Entities;

public class ProviderTests
{
    private static readonly Guid Id = Guid.NewGuid();
    private static readonly Email Email = Email.From("example@email.com").Value;
    private static readonly CompanyName CompanyName = CompanyName.From("Hammermen dev.").Value;
    private static readonly PhoneNumber PhoneNumber = PhoneNumber.From("+3806894583948").Value;

    [Fact]
    public void Should_CreateProvider_When_CreatingProviderWithId()
    {
        //Act
        var providerResult = Provider.Create(Id, CompanyName, PhoneNumber, Email);

        //Assert
        AssertProviderCreatedSuccessfully(providerResult);
        providerResult.Value.Id.Should().Be(Id);
    }

    [Fact]
    public void Should_CreateProvider_When_CreatingProviderWithoutId()
    {
        //Act
        var providerResult = Provider.Create(CompanyName, PhoneNumber, Email);

        //Assert
        AssertProviderCreatedSuccessfully(providerResult);
        providerResult.Value.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Should_ReturnFailedResult_WhenIdIsEmptyGuid()
    {
        //Arrange
        var id = Guid.Empty;

        //Act
        var providerResult = Provider.Create(id, CompanyName, PhoneNumber, Email);

        //Assert
        providerResult.AssertIsFailed(1);
    }


    private void AssertProviderCreatedSuccessfully(Result<Provider> providerResult)
    {
        providerResult.IsSuccess.Should().BeTrue();
        var provider = providerResult.Value;

        provider.CompanyName.Should().BeEquivalentTo(CompanyName);
        provider.PhoneNumber.Should().Be(PhoneNumber);
        provider.Email.Should().BeEquivalentTo(Email);
    }
}