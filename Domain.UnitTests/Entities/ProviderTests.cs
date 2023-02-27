using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using FluentResults;

namespace Domain.UnitTests.Entities;

public class ProviderTests
{
    private static readonly Guid TestId = Guid.NewGuid();
    private static readonly Email TestEmail = Email.From("example@email.com").Value;
    private static readonly CompanyName TestCompanyName = CompanyName.From("Hammermen dev.").Value;
    private static readonly string TestPhoneNumber = "+3806894583948";

    [Fact]
    public void Should_ReturnFailedResult_When_CompanyNameIsNull()
    {
        //Arrange
        CompanyName? companyName = null;
        
        //Act
        var sut = Provider.Create(TestId, companyName, TestPhoneNumber, TestEmail);
        
        //Assert
        AssertHasOneError(sut);
    }

    [Fact]
    public void Should_ReturnFailedResult_When_PhoneNumberIsNull()
    {
        //Arrange
        string? phoneNumber = null;
        
        //Act
        var sut = Provider.Create(TestId, TestCompanyName, phoneNumber, TestEmail);
        
        //Assert
        AssertHasOneError(sut);
    }
    
    [Fact]
    public void Should_ReturnFailedResult_When_EmailIsNull()
    {
        //Arrange 
        Email? email = null;
        
        //Act
        var sut = Provider.Create(TestId, TestCompanyName, TestPhoneNumber, email);
            
        //Assert
        AssertHasOneError(sut);
    }

    [Fact]
    public void Should_CreateProvider_When_AllArgumentsAreValid()
    {
        //Act
        var sut = Provider.Create(TestId, TestCompanyName, TestPhoneNumber, TestEmail).Value;
        
        //Assert
        sut.Id.Should().Be(TestId);
        sut.CompanyName.Should().BeEquivalentTo(TestCompanyName);
        sut.PhoneNumber.Should().Be(TestPhoneNumber);
        sut.Email.Should().BeEquivalentTo(TestEmail);
    }
    
    private void AssertHasOneError(Result<Provider> result)
    {
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(1);
    }
}