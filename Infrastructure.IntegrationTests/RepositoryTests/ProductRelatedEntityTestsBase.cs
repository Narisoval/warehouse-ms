using System.Collections.Immutable;
using Domain.Entities;
using Domain.Primitives;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests.RepositoryTests;

public abstract class ProductRelatedEntityTestsBase<TEntity, TRepository> : RepositoryTestsBase<TEntity, TRepository>
    where TRepository : class, IRepository<TEntity>
    where TEntity : Entity
{
    protected WarehouseDbContext Context;

    public ProductRelatedEntityTestsBase(DatabaseFixture fixture) : base(fixture)
    {
        Context = fixture.Context;
    }

    [Fact]
    public async Task Should_DeleteAllRelatedProducts_When_CategoryIsDeleted()
    {
        //Arrange
        var randomEntity = GetRandomSeededEntity();
        var relatedProductsBeforeEntityDeletion = await GetRelatedProducts(randomEntity.Id);

        //Act
        var wasEntityDeleted = await Repository.Remove(randomEntity.Id);
        await UnitOfWork.Complete();
        var relatedProductsAfterEntityDeletion = await GetRelatedProducts(randomEntity.Id);

        //Assert
        relatedProductsBeforeEntityDeletion
            .Should()
            .NotBeEmpty("There must be some products related to the brand under test");

        wasEntityDeleted.Should().BeTrue();
        relatedProductsAfterEntityDeletion.Should().BeEmpty();

        //Verify
        await EnsureEntitiesExists(randomEntity);
    }

    private async Task<IReadOnlyCollection<Product>> GetRelatedProducts(Guid entityId)
    {
        var allProducts = await Context.Products.AsNoTracking().ToListAsync();

        if (typeof(TRepository) == typeof(ICategoryRepository))
            return allProducts.Where(product => product.CategoryId == entityId).ToImmutableList();

        if (typeof(TRepository) == typeof(IBrandRepository))
            return allProducts.Where(product => product.BrandId == entityId).ToImmutableList();

        if (typeof(TRepository) == typeof(IProviderRepository))
            return allProducts.Where(product => product.ProviderId == entityId).ToImmutableList();

        throw new InvalidOperationException($"Invalid repository type: {typeof(TRepository).Name}");
    }
}