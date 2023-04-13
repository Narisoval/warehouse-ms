using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using FluentResults;

namespace Domain.UnitTests.Entities;

public class BrandTests
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly BrandName _brandName = BrandName.From("BrandName").Value;
    private readonly Image _image = Image.From("https://image.jpg").Value;
    private readonly BrandDescription _description = BrandDescription.From("This is a brand description").Value;

    [Fact]
    public void Should_Create_Brand_When_All_Properties_Are_Provided()
    {
        //Act
        var brandResult = Brand.Create(_id, _brandName, _image, _description);

        //Assert
        brandResult.Value.Id.Should().Be(_id);
        AssertBrandCreatedCorrectly(brandResult);
    }

    [Fact]
    public void Should_Create_Brand_When_IdIsNotProvided()
    {
        //Act
        var brandResult = Brand.Create(_brandName, _image, _description);
    
        //Assert
        AssertBrandCreatedCorrectly(brandResult);
    }
    
    [Fact]
    public void Should_ReturnFailedResult_When_IdIsEmptyGuid()
    {
        //Act
        var brandResult = Brand.Create(Guid.Empty, _brandName, _image, _description);
    
        //Assert
        brandResult.AssertIsFailed(1);
    }
    
    private void AssertBrandCreatedCorrectly(Result<Brand> brandResult)
    { 
        brandResult.IsSuccess.Should().BeTrue();
        
        var brand = brandResult.Value;
        
        Assert.Equal(_brandName, brand.Name);
        Assert.Equal(_image, brand.Image);
        Assert.Equal(_description, brand.Description);
        
    }
}