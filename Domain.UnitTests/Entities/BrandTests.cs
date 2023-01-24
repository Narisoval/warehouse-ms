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
        var brand = Brand.Create(id, brandName, brandImage, brandDescription);
    
        //Assert
        Assert.Equal(id, brand.Id);
        Assert.Equal(brandName, brand.BrandName);
        Assert.Equal(brandImage, brand.BrandImage);
        Assert.Equal(brandDescription, brand.Description);
    } 
    
    [Fact]
    public void Should_ChangeBrandDescription_When_Called()
    {
        //Arrange
        var sut = BrandsFixture.GetBrands().ElementAt(0);
        var newDescription = BrandDescription.From("A great brand which offers great products");
        //Act
        sut.ChangeDescription(newDescription);
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
        sut.ChangeImage(newImage);
        //Assert
        sut.BrandImage.Should().Be(newImage);
    }
}