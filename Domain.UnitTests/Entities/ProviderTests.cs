using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class ProviderTests
{
    [Fact]
    public void Should_ChangeCompanyName_WhenCalled()
    {
        var sut = ProviderFixture.CreateProvider();
        var oldCompanyName = sut.CompanyName;
        // Act
        var newCompanyName = "New Company Name";
        sut.ChangeCompanyName(newCompanyName);

        // Assert
        sut.CompanyName.Should().BeEquivalentTo(newCompanyName);
        sut.CompanyName.Should().NotBeEquivalentTo(oldCompanyName);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeCompanyNameArgumentIsNull()
    {
        // Arrange
        var sut = ProviderFixture.CreateProvider();
        string? newCompanyName = null;
        // Act & Assert
        
        Assert.Throws<ArgumentNullException>(() => sut.ChangeCompanyName(newCompanyName));
    }
 
    [Fact]
    public void Should_ChangePhoneNumber_When_Called()
    {
        // Arrange
        var sut = ProviderFixture.CreateProvider();
        var oldPhoneNumber = sut.PhoneNumber;
        var newPhoneNumber = "666-666-6666";

        // Act
        sut.ChangePhoneNumber(newPhoneNumber);

        // Assert
        sut.PhoneNumber.Should().BeEquivalentTo(newPhoneNumber);
        sut.PhoneNumber.Should().NotBeEquivalentTo(oldPhoneNumber);
    }
    
    [Fact]
    public void Should_ThrowException_When_ChangePhoneNumberArgumentIsNull()
    {
        // Arrange
        var sut = ProviderFixture.CreateProvider();
        string? newPhoneNumber = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangePhoneNumber(newPhoneNumber));
    }

    [Fact]
    public void Should_ChangeEmail_When_Called()
    {
        // Arrange
        var sut = ProviderFixture.CreateProvider();
        var newEmail = Email.From("exampleee@example.com");
        var oldEmail = sut.Email;

        // Act
        sut.ChangeEmail(newEmail);

        // Assert
        sut.Email.Should().BeEquivalentTo(newEmail);
        sut.Email.Should().NotBeEquivalentTo(oldEmail);
    }
    
    [Fact]
    public void Should_ThrowException_When_ChangeEmailArgumentIsNull()
    {
        // Arrange
        var sut = ProviderFixture.CreateProvider();
        Email? newEmail = null; 

        // Act && Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeEmail(newEmail));
    } 
}