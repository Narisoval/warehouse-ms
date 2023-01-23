using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class BrandTests
{
    [Fact]
    public void Should_SetDescriptionToNull_When_RemoveBrandDescription()
    {
        //Arrange
        var sut = BrandsFixture.GetBrands().ElementAt(0);
        //Act
        sut.RemoveBrandDescription();
        //Assert
        sut.Description.Should().Be(null);
    }
    
    [Fact]
    public void Should_SetImageToNull_When_RemoveBrandImage()
    {
        //Arrange
        var sut = BrandsFixture.GetBrands().ElementAt(0);
        //Act
        sut.RemoveBrandImage();
        //Assert
        sut.BrandImage.Should().Be(null);
    }
    
    [Fact]
    public void Should_ChangeBrandDescription_When_ChangeBrandDescription()
    {
        //Arrange
        var sut = BrandsFixture.GetBrands().ElementAt(0);
        var newDescription = BrandDescription.From("A great brand which offers great products");
        //Act
        sut.ChangeBrandDescription(newDescription);
        //Assert
        sut.Description.Should().Be(newDescription);
    }
    
    [Fact]
    public void Should_ChangeBrandImage_When_ChangeBrandImage()
    {
        //Arrange
        var sut = BrandsFixture.GetBrands().ElementAt(0);
        var newImage = Image.From("https://cat.png");
        //Act
        sut.ChangeBrandImage(newImage);
        //Assert
        sut.BrandImage.Should().Be(newImage);
    }
}