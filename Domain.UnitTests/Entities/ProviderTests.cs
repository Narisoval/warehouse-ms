using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class ProviderTests
{
    private static readonly Guid TestId = Guid.NewGuid();
    private static readonly Email TestEmail = Email.From("example@email.com");
    private static readonly CompanyName TestCompanyName = CompanyName.From("Hammermen dev.");
    private static readonly string TestPhoneNumber = "+3806894583948";

    [Fact]
    public void Should_ThrowException_When_CompanyNameIsNull()
    {
        CompanyName? companyName = null;
        Assert.Throws<ArgumentNullException>(() => Provider.Create(TestId, companyName, TestPhoneNumber, TestEmail));
    }

    [Fact]
    public void Should_ThrowException_When_PhoneNumberIsNull()
    {
        string? phoneNumber = null;
        Assert.Throws<ArgumentNullException>(() => Provider.Create(TestId, TestCompanyName, phoneNumber, TestEmail));
    }

    [Fact]
    public void Should_ThrowException_When_EmailIsNull()
    {
        Email? email = null;
        Assert.Throws<ArgumentNullException>(() => Provider.Create(TestId, TestCompanyName, TestPhoneNumber, email));
    }

    [Fact]
    public void Should_CreateProvider_When_AllArgumentsAreValid()
    {
        //Act
        var sut = Provider.Create(TestId, TestCompanyName, TestPhoneNumber, TestEmail);
        
        //Assert
        sut.Id.Should().Be(TestId);
        sut.CompanyName.Should().BeEquivalentTo(TestCompanyName);
        sut.PhoneNumber.Should().Be(TestPhoneNumber);
        sut.Email.Should().BeEquivalentTo(TestEmail);
    }
}