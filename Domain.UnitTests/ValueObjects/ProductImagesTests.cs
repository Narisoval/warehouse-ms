using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class ProductImagesTests
{
    [Fact]
    public void Should_ThrowException_When_ThereIsMoreThanMaximumMainImages()
    {
        //Arrange
        var images = GenerateProductImages(ProductImages.MaxAmountOfMainImages+1,true);
        
        //Act && Assert
        Assert.Throws<TooManyMainImagesDomainException>(() => ProductImages.From(images));
    }
    
    [Fact]
    public void Should_ThrowException_When_ThereIsNoMainImage()
    {
        //Arrange
        var images = GenerateProductImages(15, false);

        //Act && Assert
        Assert.Throws<NoMainImagesDomainException>(() => ProductImages.From(images));
    }

    private List<ProductImage> GenerateProductImages(int amount, bool isMain)
    {
        var images = new List<ProductImage>();
        //Arrange
        for (int i = 0; i <= amount; i++)
        {
            var img = ProductImage.Create(Guid.NewGuid(), Image.From($"https://{Guid.NewGuid()}.png"), isMain);
            images.Add(img);
        }

        return images;
    }
    
}