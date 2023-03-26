using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests.RepositoryTests;

public class CategoryRepositoryTests : ProductRelatedEntityTestsBase<Category, ICategoryRepository>
{
    public CategoryRepositoryTests(DatabaseFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public override async Task Should_UpdateEntity_When_Called()
    {
        // Arrange
        var seededCategory = GetRandomSeededEntity();

        // Detach the seeded entity to avoid tracking issues in EF Core
        Context.Entry(seededCategory).State = EntityState.Detached;
        
        // Act
        var updatedCategory = Category.Create(
            seededCategory.Id, CategoryName.From("UpdatedCategory").Value).Value;

        var result = await Repository.Update(updatedCategory);
        
        await UnitOfWork.Complete(); 
        
        var fetchedCategory = await Repository.Get(seededCategory.Id);
        
        // Assert
        result.Should().BeTrue();
        fetchedCategory.Should().BeEquivalentTo(updatedCategory);
    }

    [Fact]
    public override async Task Should_ReturnFalse_When_UpdatingNonExistingEntity()
    {
        // Arrange
        var nonExistingCategory = Category.Create(
            Guid.NewGuid(), CategoryName.From("NonExistingCategory").Value).Value;

        // Act
        var result = await Repository.Update(nonExistingCategory);
        await UnitOfWork.Complete();
        var fetchedNonExistentCategory = await Repository.Get(nonExistingCategory.Id);
        
        // Assert
        result.Should().BeFalse();
        fetchedNonExistentCategory.Should().BeNull();
    }
    
    [Fact]
    public override async Task Should_NotChangeAnything_When_UpdatingEntityWithNoChanges()
    {
        // Arrange
        var seededCategory = GetRandomSeededEntity();
        
        // Detach the entity from the DbContext
        Context.Entry(seededCategory).State = EntityState.Detached;
        
        // Act
        var result = await Repository.Update(seededCategory);
        
        await UnitOfWork.Complete(); 
        
        var fetchedCategory = await Repository.Get(seededCategory.Id);
        
        // Assert
        result.Should().BeTrue();
        fetchedCategory.Should().BeEquivalentTo(seededCategory);
    }
}