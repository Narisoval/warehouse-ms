using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public class ImageTests
{
    [Theory]
    [InlineData("https://storage.googleapis.com/eCommerceApp/kitten.docx")]
    [InlineData("https://storage.googleapis.com/eCommerceApp/kitten.jjj")]
    [InlineData("https://storage.googleapis.com/eCommerceApp/kitten.hjkl")]
    [InlineData("https://storage.googleapis.com/eCommerceApp/kitten.sh")]
    [InlineData("httttps://storage.googleapis.com/eCommerceApp/kitten.jpg")]
    [InlineData("https://storage.  .com/eCommerceApp/kitten.jpg")]
    [InlineData("https://happyme nin  socks.png")]
    public void Should_ThrowException_When_ImageUrlIsIncorrect(string? image)
    {
        Assert.Throws<FormatException>(() => Domain.ValueObjects.Image.From(image));
    }
    
    [Fact]
    public void Should_ThrowException_When_ImageUrlIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Domain.ValueObjects.Image.From(null));
    }
    
    [Fact]
    public void Should_CreateImage_When_ImageUrlIsValid()
    {
        //Arrange
        string imageUrl = "https://image.jpg ";
        //Act
        var sut = Domain.ValueObjects.Image.From(imageUrl);
        //Assert
        sut.Value.Should().Be(imageUrl);
    }
    
}