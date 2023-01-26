using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class ProviderTests
{
    private static readonly Guid Id = Guid.NewGuid();
    private static readonly Email Email = Email.From("example@email.com");
    private static readonly string CompanyName = "Hammermen dev.";
    private static readonly string PhoneNumber = "+3806894583948";

    [Fact]
    public void Should_ThrowException_When_CompanyNameIsNull()
    {
        string? companyName = null;
        Assert.Throws<ArgumentNullException>(() => Provider.Create(Id, companyName, PhoneNumber, Email));
    }

    [Fact]
    public void Should_ThrowException_When_PhoneNumberIsNull()
    {
        string? phoneNumber = null;
        Assert.Throws<ArgumentNullException>(() => Provider.Create(Id, CompanyName, phoneNumber, Email));
    }

    [Fact]
    public void Should_ThrowException_When_EmailIsNull()
    {
        Email? email = null;
        Assert.Throws<ArgumentNullException>(() => Provider.Create(Id, CompanyName, PhoneNumber, email));
    }

    [Fact]
    public void Should_CreateProvider_When_AllArgumentsAreValid()
    {
        //Act
        var sut = Provider.Create(Id, CompanyName, PhoneNumber, Email);
        
        //Assert
        sut.Id.Should().Be(Id);
        sut.CompanyName.Should().BeEquivalentTo(CompanyName);
        sut.PhoneNumber.Should().Be(PhoneNumber);
        sut.Email.Should().BeEquivalentTo(Email);
    }
}