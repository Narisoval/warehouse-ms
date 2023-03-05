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
    public void Should_CreateProvider_When_CreatingProviderWithId()
    {
        //Act
        var providerResult = Provider.Create(TestId, TestCompanyName, TestPhoneNumber, TestEmail);

        //Assert
        AssertProviderCreatedSuccessfully(providerResult);
        providerResult.Value.Id.Should().Be(TestId);
    }

    [Fact]
    public void Should_CreateProvider_When_CreatingProviderWithoutId()
    {
        //Act
        var providerResult = Provider.Create(TestCompanyName, TestPhoneNumber, TestEmail);

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
        var providerResult = Provider.Create(id, TestCompanyName, TestPhoneNumber, TestEmail);

        //Assert
        providerResult.AssertIsFailed(1);
    }


    [Fact]
    public void Should_ReturnFailedResult_When_OneOfTheArgumentsIsNull()
    {
        //Arrange
        var arguments = new List<object?>
        {
            TestCompanyName,
            TestPhoneNumber,
            TestEmail
        };

        for (int i = 0; i < arguments.Count; i++)
        {
            arguments[i] = null;
            
            //Act
            var providerCreatedWithId = CreateProviderWithId(arguments);
            var providerCreatedWithoutId = CreateProviderWithoutId(arguments);
            
            //Assert
            providerCreatedWithId.AssertIsFailed(i+1);
            providerCreatedWithoutId.AssertIsFailed(i+1);
        }
    }
    
    private Result<Provider> CreateProviderWithId(List<object?> arguments)
    {
        return Provider.Create(
            TestId, 
            (CompanyName?)arguments[0],
            (string?)arguments[1],
            (Email?)arguments[2]);
    }
    
    private Result<Provider> CreateProviderWithoutId(List<object?> arguments)
    {
        return Provider.Create(
            (CompanyName?)arguments[0],
            (string?)arguments[1],
            (Email?)arguments[2]);
    }

    [Fact]
    public void Should_ReturnFailedResult_When_PhoneNumberIsNull()
    {
        //Arrange
        string? phoneNumber = null;
        
        //Act
        var sut = Provider.Create(TestId, TestCompanyName, phoneNumber, TestEmail);
        
        //Assert
        sut.AssertIsFailed(1);
    }
    
    [Fact]
    public void Should_ReturnFailedResult_When_EmailIsNull()
    {
        //Arrange 
        Email? email = null;
        
        //Act
        var sut = Provider.Create(TestId, TestCompanyName, TestPhoneNumber, email);
            
        //Assert
        sut.AssertIsFailed(1);
    }

    private void AssertProviderCreatedSuccessfully(Result<Provider> providerResult)
    {
        providerResult.IsSuccess.Should().BeTrue();
        var provider = providerResult.Value;

        provider.CompanyName.Should().BeEquivalentTo(TestCompanyName);
        provider.PhoneNumber.Should().Be(TestPhoneNumber);
        provider.Email.Should().BeEquivalentTo(TestEmail);
    }
}