using Domain.Entities;
using Domain.UnitTests.Fixtures;
using FluentAssertions;

namespace Domain.UnitTests.Entities.ProductTests;

public class ProductProviderTests
{
    [Fact]
    public void Should_ChangeProvider_WhenCalled()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        var originalProviderId = sut.ProviderId;

        var newProvider = ProvidersFixture.GetTestProvider();
        var newProviderId = newProvider.Id;

        //Act 
        sut.ChangeProvider(newProvider);

        //Assert
        sut.Provider.Should().Be(newProvider);
        sut.ProviderId.Should().Be(newProviderId);
        sut.ProviderId.Should().NotBe(originalProviderId);
    }

    [Fact]
    public void Should_SetProviderNull_When_ChangeProviderArgumentIsNull()
    {
        //Arrange
        var sut = ProductsFixture.GetTestProduct();
        Provider? newProvider = null;
        
        //Act 
        sut.ChangeProvider(newProvider);

        //Assert
        Assert.Null(sut.Provider);
    }
}