using System.Collections.Immutable;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests.RepositoryTests;

public class BrandRepositoryTests : ProductRelatedEntityTestsBase<Brand, IBrandRepository>
{
    public BrandRepositoryTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    private async Task<IReadOnlyCollection<Product>> GetRelatedProducts(Guid brandId)
    {
        var allProducts = await UnitOfWork.Products.GetAll();
        return allProducts.Where(pr => pr.BrandId == brandId).ToImmutableList();
    }

    [Fact]
    public override async Task Should_UpdateEntity_When_Called()
    {
        // Arrange
        var seededBrand = GetRandomSeededEntity();

        // Detach the seeded entity to avoid tracking issues in EF Core
        Context.Entry(seededBrand).State = EntityState.Detached;

        // Act
        var updatedBrand = Brand.Create(
            seededBrand.Id,
            BrandName.From("Updated Brand Name").Value,
            Image.From("https://updatedimageurl.png").Value,
            BrandDescription.From("These socks are still as happy as you will be while wearing them").Value).Value;

        var result = await Repository.Update(updatedBrand);
        
        await UnitOfWork.Complete(); 
        
        var fetchedBrand = await Repository.Get(updatedBrand.Id);
        
        // Assert
        result.Should().BeTrue();
        fetchedBrand.Should().BeEquivalentTo(updatedBrand);
    }
    
    [Fact]
    public override async Task Should_ReturnFalse_When_UpdatingNonExistingEntity()
    {
        // Arrange
        var nonExistingBrand = Brand.Create(
            Guid.NewGuid(),
            BrandName.From("NonExisting Brand Name").Value,
            Image.From("https://nonexistingimageurl.png").Value,
            BrandDescription.From("Non-existing brand description").Value).Value;

        // Act
        var result = await Repository.Update(nonExistingBrand);
        await UnitOfWork.Complete();
        var fetchedNonExistingBrand = await Repository.Get(nonExistingBrand.Id);

        // Assert
        result.Should().BeFalse();
        fetchedNonExistingBrand.Should().BeNull();
    }

    [Fact]
    public override async Task Should_NotChangeAnything_When_UpdatingEntityWithNoChanges()
    {
        // Arrange
        var seededBrand = GetRandomSeededEntity(); 

        // Act
        var result = await Repository.Update(seededBrand);
        await UnitOfWork.Complete();
        var fetchedBrand = await Repository.Get(seededBrand.Id);
        
        // Assert
        result.Should().BeTrue();
        fetchedBrand.Should().BeEquivalentTo(seededBrand); 
    }
}