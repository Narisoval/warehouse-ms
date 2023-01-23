using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class BrandTests
{
    // CONSTRUCTORS
    [Fact]
    public void Should_Create_Brand_When_All_Properties_Are_Provided()
    {
        //Arrange
        var id = Guid.NewGuid();
        var brandName = "BrandName";
        var brandImage = Image.From("https://image.jpg");
        var brandDescription = BrandDescription.From("This is a brand description");
    
        //Act
        var brand = new Brand(id, brandName, brandImage, brandDescription);
    
        //Assert
        Assert.Equal(id, brand.Id);
        Assert.Equal(brandName, brand.BrandName);
        Assert.Equal(brandImage, brand.BrandImage);
        Assert.Equal(brandDescription, brand.Description);
    } 
    
    [Fact]
    public void Should_CreateBrand_When_BrandDescriptionIsNotProvided()
    {
        // Arrange
        var id = Guid.NewGuid();
        var brandName = "Nike";
        var brandImage = Image.From("https://example.com/nike.jpg");

        // Act
        var brand = new Brand(id, brandName, brandImage);

        // Assert
        Assert.Equal(id, brand.Id);
        Assert.Equal(brandName, brand.BrandName);
        Assert.Equal(brandImage, brand.BrandImage);
        Assert.Null(brand.Description);
    } 
    
 
    [Fact]
    public void Should_Create_Brand_When_OnlyBrandNameIsProvided()
    {
        //Arrange
        var id = Guid.NewGuid();
        var brandName = "BrandName";
    
        //Act
        var brand = new Brand(id, brandName);
    
        //Assert
        Assert.Equal(id, brand.Id);
        Assert.Equal(brandName, brand.BrandName);
        Assert.Null(brand.BrandImage);
        Assert.Null(brand.Description);
    } 
    
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
    public void Should_ChangeBrandDescription_When_Called()
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
    public void Should_ChangeBrandImage_When_Called()
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