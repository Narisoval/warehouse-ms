using Domain.Entities;
using Domain.Primitives;
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
        Assert.Equal(_id, brandResult.Value.Id);
        AssertBrandCreatedCorrectly(brandResult);
    }

    private void AssertBrandCreatedCorrectly(Result<Brand> brandResult)
    { 
        brandResult.IsSuccess.Should().BeTrue();
        
        var brand = brandResult.Value;
        
        Assert.Equal(_brandName, brand.Name);
        Assert.Equal(_image, brand.Image);
        Assert.Equal(_description, brand.Description);
        
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
    [Fact]
    public void Should_ReturnFailedResult_When_SomeArgumentsAreNull()
    {
        // Arrange
        var arguments = new List<ValueObject?> { _brandName, _image, _description };

        // Act and Assert
        for (int i = 0; i < arguments.Count; i++)
        {
            arguments[i] = null;
            var brandResultWithoutId = CreateBrandWithoutId(arguments);
            var brandResultWithId = CreateBrandWithId(arguments);
                
            brandResultWithId.AssertIsFailed(i+1);
            brandResultWithoutId.AssertIsFailed(i+1);
        }
    }

    private Result<Brand> CreateBrandWithId(List<ValueObject?> arguments)
    {
        return Brand.Create(
            _id,
            (BrandName?)arguments[0],
            (Image?)arguments[1]!,
            (BrandDescription?)arguments[2]!);    
    }
    
    private Result<Brand> CreateBrandWithoutId(List<ValueObject?> arguments)
    {
        return Brand.Create(
            (BrandName?)arguments[0],
            (Image?)arguments[1]!,
            (BrandDescription?)arguments[2]!);    
    }

}