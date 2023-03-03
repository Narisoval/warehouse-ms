using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using FluentResults;

namespace Domain.UnitTests.ValueObjects;

public class ProductImagesTests
{
    [Fact]
    public void Should_ReturnFailedResult_When_ThereIsMoreThanMaximumMainImages()
    {
        //Arrange
        var images = GenerateProductImages(ProductImages.MaxAmountOfMainImages+1,true);
        
        //Act 
        var sut = ProductImages.From(images);
        
        //Assert
        AssertFailedResultWithOneError(sut);
    }
    
    [Fact]
    public void Should_ReturnFailedResult_When_ThereIsNoMainImage()
    {
        //Arrange
        var images = GenerateProductImages(15, false);

        //Act 
        var sut = ProductImages.From(images);
        
        //Assert
        AssertFailedResultWithOneError(sut);
    }

    private void AssertFailedResultWithOneError(Result<ProductImages> result)
    {
        result.IsFailed.Should().BeTrue();
        result.Errors.Count.Should().Be(1);
    }
    
    private List<ProductImage> GenerateProductImages(int amount, bool isMain)
    {
        var images = new List<ProductImage>();
        //Arrange
        for (int i = 0; i <= amount; i++)
        {
            var img = ProductImage.Create(
                Guid.NewGuid(), 
                Image.From($"https://{Guid.NewGuid()}.png").Value, isMain).Value;
            images.Add(img);
        }

        return images;
    }
    
}