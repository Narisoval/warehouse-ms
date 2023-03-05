using Domain.ValueObjects;
using FluentAssertions;

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
        Assert.Equal(email, result.Value.Value);
    }

    [Theory]
    [InlineData("example")]
    [InlineData("username@example")]
    [InlineData("username@.com")]
    [InlineData("@example.com")]
    [InlineData("example@example.com....")]
    public void Should_ReturnFaultedResult_When_FormatIsIncorrect(string email)
    {
        //Act
        var emailResult = Email.From(email);
        
        //Assert
        emailResult.AssertIsFailed(1); 
    }
    
    [Fact]
    public void Should_ReturnFaultedResult_When_EmailIsNull()
    {
        // Arrange
        string? email = null;

        // Act
        var emailResult = Email.From(email);
        
        //Assert
        emailResult.AssertIsFailed(1);
    }
}