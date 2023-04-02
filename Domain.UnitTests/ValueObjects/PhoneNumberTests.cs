using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

using FluentAssertions;
using FluentResults;
using Xunit;

public class PhoneNumberTests
{
    [Fact]
    public void Should_ReturnSuccessResult_When_PhoneNumberIsValid()
    {
        // Arrange
        string validPhoneNumber = "1-800-123456";

        // Act
        Result<PhoneNumber> result = PhoneNumber.From(validPhoneNumber);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(validPhoneNumber);
    }

    [Fact]
    public void Should_ReturnErrorResult_When_PhoneNumberIsNull()
    {
        // Arrange
        string nullPhoneNumber = null!;

        // Act
        Result<PhoneNumber> result = PhoneNumber.From(nullPhoneNumber);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void Should_ReturnErrorResult_When_PhoneNumberHasLessThanFiveDigits()
    {
        // Arrange
        string invalidPhoneNumber = "1-234";

        // Act
        Result<PhoneNumber> result = PhoneNumber.From(invalidPhoneNumber);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void Should_ReturnErrorResult_When_PhoneNumberExceedsMaxLength()
    {
        // Arrange
        string invalidPhoneNumber = "1-800-12345678901234567890";

        // Act
        Result<PhoneNumber> result = PhoneNumber.From(invalidPhoneNumber);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public void Should_BeEqual_When_ComparingTwoEqualPhoneNumbers()
    {
        // Arrange
        PhoneNumber phoneNumber1 = PhoneNumber.From("1-800-123456").Value;
        PhoneNumber phoneNumber2 = PhoneNumber.From("1-800-123456").Value;

        // Assert
        phoneNumber1.Should().Be(phoneNumber2);
    }

    [Fact]
    public void Should_NotBeEqual_When_ComparingTwoDifferentPhoneNumbers()
    {
        // Arrange
        PhoneNumber phoneNumber1 = PhoneNumber.From("1-800-123456").Value;
        PhoneNumber phoneNumber2 = PhoneNumber.From("1-800-654321").Value;

        // Assert
        phoneNumber1.Should().NotBe(phoneNumber2);
    }
}
