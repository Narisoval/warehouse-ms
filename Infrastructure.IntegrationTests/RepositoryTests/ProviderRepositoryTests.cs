using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests.RepositoryTests;

public class ProviderRepositoryTests : ProductRelatedEntityTestsBase<Provider,IProviderRepository>
{
    public ProviderRepositoryTests(DatabaseFixture fixture) : base(fixture)
    {
    }
    
    [Fact]
    public override async Task Should_UpdateEntity_When_Called()
    {
        // Arrange
        var seededProvider = GetRandomSeededEntity();

        // Detach the seeded entity to avoid tracking issues in EF Core
        Context.Entry(seededProvider).State = EntityState.Detached;

        var updatedProvider = Provider.Create(
            seededProvider.Id,
            CompanyName.From("Updated Company Name").Value,
            "+38069355555",
            Email.From("updatedemail@gmail.com").Value).Value;
    
        // Act
        var result = await Repository.Update(updatedProvider);
        await UnitOfWork.Complete();
        var fetchedProvider = await Repository.Get(updatedProvider.Id);
    
        // Assert
        result.Should().BeTrue();
        fetchedProvider.Should().BeEquivalentTo(updatedProvider);
    }
    
    [Fact]
    public override async Task Should_ReturnFalse_When_UpdatingNonExistingEntity()
    {
        // Arrange
        var nonExistingProvider = Provider.Create(
            Guid.NewGuid(),
            CompanyName.From("Non-existing Company Name").Value,
            "+38069355555",
            Email.From("nonexistingemail@gmail.com").Value).Value;
    
        // Act
        var result = await Repository.Update(nonExistingProvider);
        await UnitOfWork.Complete();
        var fetchedNonExistentProvider = await Repository.Get(nonExistingProvider.Id);
        
        // Assert
        result.Should().BeFalse();
        fetchedNonExistentProvider.Should().BeNull();
    }
    
    [Fact]
    public override async Task Should_NotChangeAnything_When_UpdatingEntityWithNoChanges()
    {
        // Arrange
        var seededProvider = GetRandomSeededEntity();

        // Act
        var result = await Repository.Update(seededProvider);
        var fetchedProvider = await Repository.Get(seededProvider.Id);
        
        // Assert
        result.Should().BeTrue();
        fetchedProvider.Should().BeEquivalentTo(seededProvider);
    }
    
}