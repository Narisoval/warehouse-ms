using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities.ProductTests;

public class ProductImagesTests
{
    [Fact]
    public void Should_ChangeAllImages_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var newImages = ProductImagesFixture.GetTestProductImages();

        //Act 
        sut.ChangeAllImages(newImages.Value);

        //Assert
        sut.Images.Should().BeEquivalentTo(newImages.Value);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeAllImagesArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        IList<ProductImage>? newProductImages = null;
        
        //Act && Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeAllImages(newProductImages));
    }

    [Fact]
    public void Should_ThrowException_WhenChangeImageArgumentIsWrongId()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var newImageId = Guid.NewGuid();
        var newImage = Image.From("https://abc.png");
        
        //Act && Assert
        Assert.Throws<ArgumentException>(() => sut.ChangeImage(newImageId,newImage));  
    }

    [Fact]
    public void Should_ThrowException_When_ChangeImageIdIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var imageToChangeTo = Image.From("https://a.png");
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeImage(null,imageToChangeTo));
    }
    
    [Fact]
    public void Should_ThrowException_When_ChangeImageImageIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeImage(Guid.NewGuid(),null));
    }

    [Fact]
    public void Should_ChangeRightImage_When_ChangeImageWhenCalled()
    {
        //Arrange
        var productImages = ProductImagesFixture.GetTestProductImages();
        var sut = ProductsFixture.GetTestProduct(productImages);
        var newImage = Image.From("https://abc.png");
        
        //Act
        foreach (var currProductImage in sut.Images)
        {
            var initialImage = currProductImage.Image;
            sut.ChangeImage(currProductImage.Id, newImage);
            
            //Assert
            currProductImage.Image.Should().BeEquivalentTo(newImage);
            
            currProductImage.Image = initialImage;
        }
    }

    [Fact]
    public void Should_NotChangeOtherImages_WhenChangeImageIsCalled()
    {
        //Arrange
        var productImages = ProductImagesFixture.GetTestProductImages();
        var sut = ProductsFixture.GetTestProduct(productImages);
        var newImage = Image.From("https://abc.png");
        
        foreach (var currProductImage in sut.Images)
        {
            var initialImage = currProductImage.Image;
            
            //Act
            sut.ChangeImage(currProductImage.Id,newImage);
            
            var otherImages = sut.Images.Where(image => image.Id != currProductImage.Id);
            foreach(var otherImage in otherImages)
            {
                //Assert
                otherImage.Image.Should().NotBeEquivalentTo(newImage);
            }
            currProductImage.Image = initialImage;
        }
    }

    [Fact]
    public void Should_GetMainImage_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var mainImageFromImages = sut.Images.First(img => img.IsMain);
        
        //Act
        var mainImageFromMethod = sut.GetMainImage();
        
        //Assert
        mainImageFromMethod.IsMain.Should().BeTrue();
        mainImageFromImages.Should().Be(mainImageFromMethod);

    }
    
    [Fact]
    public void Should_ChangeMainImage_When_Called()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var oldMainImage = sut.GetMainImage().Image;
        var newMainImage = Image.From("https://new.png");
        
        //Act
        sut.ChangeMainImage(newMainImage); 
        var currentMainImage = sut.GetMainImage().Image;
        
        //Assert
        currentMainImage.Should().Be(newMainImage);
        currentMainImage.Should().NotBe(oldMainImage);
    }
    
    [Fact]
    public void Should_ThrowException_When_ChangeMainImageArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        Image? newMainImage = null; 
        
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeMainImage(newMainImage)); 
    }
}