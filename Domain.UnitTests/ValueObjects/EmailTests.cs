using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("example@email.com")]
    [InlineData("username@example.co.uk")]
    public void Should_CreateEmail_When_FormatIsCorrect(string email)
    {
        // Act
        var result = Email.From(email);

        // Assert
        Assert.Equal(email, result.Value);
    }

    [Theory]
    [InlineData("example")]
    [InlineData("username@example")]
    [InlineData("username@.com")]
    [InlineData("@example.com")]
    [InlineData("example@example.com....")]
    public void Should_ThrowArgumentException_When_FormatIsIncorrect(string email)
    {
        Assert.Throws<FormatException>(() => Email.From(email));
    }
    
    [Fact]
    public void Should_ThrowArgumentNullException_When_EmailIsNull()
    {
        // Arrange
        string? email = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Email.From(email));

    }
}