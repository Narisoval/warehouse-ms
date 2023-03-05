using FluentAssertions;
using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

    public record TestImage(string Value, int NumberOfErrors);
    public static class TextImageExtensions 
    {
        public static object[] ToObjectList(this TestImage image)
        {
            return new object[] { image };
        }
    }
public class ImageTests
{
    public static IEnumerable<object[]> GetTestImages()
    {
        yield return new TestImage("https://storage.googleapis.com/eCommerceApp/kitten.docx", 1)
            .ToObjectList();
        
        yield return new TestImage("https://storage.googleapis.com/eCommerceApp/kitten.jjj", 1).
            ToObjectList();
        
        yield return new TestImage("htt://storage.googleapis.com/eCommerceApp/kitten.sh", 2)
            .ToObjectList();
        
        yield return new TestImage("", 3).ToObjectList();
        
        yield return new TestImage(".jpg", 2).ToObjectList();
    }
    [Theory]
    [MemberData(nameof(GetTestImages))]
    public void Should_ReturnFailedResultWithCorrectNumberOfErrors_When_ImageUrlIsIncorrect(TestImage testImage)
    {
        // Act
        var sut = Image.From(testImage.Value);
        
        //Assert
        sut.AssertIsFailed(testImage.NumberOfErrors);
    }
    
    [Fact]
    public void Should_ReturnFailedResult_When_ImageUrlIsNull()
    {
        //Arrange
        string image = null!;
        
        //Act
        var sut = Image.From(image);
        
        //Assert
        sut.AssertIsFailed(1);
    }
    
    [Fact]
    public void Should_CreateImage_When_ImageUrlIsValid()
    {
        //Arrange
        string imageUrl = "https://image.jpg";
        //Act
        var sut = Image.From(imageUrl);
        //Assert
        sut.IsSuccess.Should().BeTrue();
        var imageValueObject = sut.Value;
        imageValueObject.Value.Should().Be(imageUrl);
    }
}