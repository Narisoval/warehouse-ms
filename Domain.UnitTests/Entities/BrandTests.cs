using Domain.Entities;
using Domain.UnitTests.Fixtures;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Entities;

public class BrandTests
{
    [Fact]
    public void Should_Create_Brand_When_All_Properties_Are_Provided()
    {
        //Arrange
        var id = Guid.NewGuid();
        var brandName = BrandName.From("Adidas");
        var brandImage = Image.From("https://image.jpg");
        var brandDescription = BrandDescription.From("This is a brand description");
    
        //Act
        var brand = Brand.Create(id, brandName, brandImage, brandDescription);
    
        //Assert
        Assert.Equal(id, brand.Id);
        Assert.Equal(brandName, brand.Name);
        Assert.Equal(brandImage, brand.Image);
        Assert.Equal(brandDescription, brand.Description);
    } 
    
    [Fact]
    public void Should_ChangeBrandDescription_When_Called()
    {
        //Arrange
        var sut = BrandsFixture.GetTestBrand();
        var newDescription = BrandDescription.From("A great brand which offers great products");
        //Act
        sut.ChangeDescription(newDescription);
        //Assert
        sut.Description.Should().Be(newDescription);
    }
    
    [Fact]
    public void Should_ThrowException_When_ChangeDescriptionArgumentIsNull()
    {
        //Arrange
        var sut = BrandsFixture.GetTestBrand();
        BrandDescription? newDescription = null; 
        //Act && Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeDescription(newDescription));
    }
    
    [Fact]
    public void Should_ChangeImage_When_Called()
    {
        //Arrange
        var sut = BrandsFixture.GetTestBrand();
        var newImage = Image.From("https://cat.png");
        //Act
        sut.ChangeImage(newImage);
        //Assert
        sut.Image.Should().Be(newImage);
    }

    [Fact]
    public void Should_ThrowException_When_ChangeImageArgumentIsNull()
    {
        //Arrange
        var sut = BrandsFixture.GetTestBrand();
        Image? newImage = null;
        //Act && Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeImage(newImage));
    }

    [Fact]
    public void Should_ChangeName_When_Called()
    {
        //Arrange
        var sut = BrandsFixture.GetTestBrand();
        var newBrandName = BrandName.From("BBA");
        
        //Act
        sut.ChangeName(newBrandName); 
        
        //Assert
        sut.Name.Should().Be(newBrandName);
    }
    
    [Fact]
    public void Should_ThrowException_When_ChangeNameArgumentIsNull()
    {
        //Arrange
        var sut = BrandsFixture.GetTestBrand();
        BrandName? newBrandName = null;
        
        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => sut.ChangeName(newBrandName));
    }
}