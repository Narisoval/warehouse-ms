using Domain.Entities;
using Domain.UnitTests.Fixtures;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.IntegrationTests;

public class DbContextTests : IDisposable
{
    private readonly WarehouseDbContext _context;
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<WarehouseDbContext> _contextOptions;
    
    #region ConstructorAndDispose
    public DbContextTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<WarehouseDbContext>()
            .UseSqlite(_connection)
            .Options;

        
        _context = new WarehouseDbContext(_contextOptions);

        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _connection.Dispose();
    }
    
    #endregion
    
    [Fact]
    public async void Should_AddAllEntities_When_ProductIsAdded()
    {
        // Arrange
        var testProduct = ProductsFixture.GetTestProduct();
        //Act
        await _context.Products.AddAsync(testProduct);
        
        await _context.SaveChangesAsync();
        var productFromDb = await _context.Products.AsNoTracking()
            .Include(x => x.Images)
            .Where(product => product.Id == testProduct.Id)
            .FirstAsync();
        var brandFromDb = await _context.Brands.FindAsync(testProduct.Brand.Id);
        var providerFromDb  = await _context.Providers.FindAsync(testProduct.Provider.Id);
        var categoryFromDb  = await _context.Categories.FindAsync(testProduct.Category.Id);
        
        //Assert
        productFromDb.Should().NotBeNull();
        brandFromDb.Should().NotBeNull();
        providerFromDb.Should().NotBeNull();
        categoryFromDb.Should().NotBeNull();
        productFromDb.Images.Should().NotBeNull();
        
        productFromDb.Should().Be(testProduct);
        productFromDb.Images.Should().BeEquivalentTo(testProduct.Images);
        brandFromDb.Should().Be(testProduct.Brand);
        providerFromDb.Should().Be(testProduct.Provider);
        categoryFromDb.Should().Be(testProduct.Category);
    }

    [Fact]
    public async void Should_NotDeleteAllAssociatedEntities_When_ProductIsDeleted()
    {
        // Arrange
        var testProduct = ProductsFixture.GetTestProduct();
        
        await _context.Products.AddAsync(testProduct);
        await _context.SaveChangesAsync();
        //Act
        _context.Products.Remove(testProduct);
        await _context.SaveChangesAsync();

        Product? productFromDb = await _context.FindAsync<Product>(testProduct.Id);
        var brandFromDb = await _context.FindAsync<Brand>(testProduct.Brand.Id);
        var providerFromDb  = await _context.FindAsync<Provider>(testProduct.Provider.Id);
        var categoryFromDb  = await _context.FindAsync<Category>(testProduct.Category.Id);
        
        //Assert
        productFromDb.Should().BeNull();
        brandFromDb.Should().Be(testProduct.Brand);
        providerFromDb.Should().Be(testProduct.Provider);
        categoryFromDb.Should().Be(testProduct.Category);
    }
    
    
    [Fact]
    public async void Should_ThrowException_When_ForeignKeyRestrictionsAreViolated()
    {
        //Arrange
        var testProduct = ProductsFixture.GetTestProduct();
        await _context.Products.AddAsync(testProduct);
        await _context.SaveChangesAsync();
        
        
        //Act & Assert
        var newBrand = BrandsFixture.GetTestBrand();
        testProduct.ChangeBrand(newBrand);
        _context.Products.Update(testProduct);
        var sut = async Task () => await _context.SaveChangesAsync();

        //Assert
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(sut);
    }
    
}