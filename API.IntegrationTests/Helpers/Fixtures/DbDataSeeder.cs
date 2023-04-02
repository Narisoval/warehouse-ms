using Infrastructure.Data;

namespace API.IntegrationTests.Helpers.Fixtures;

public class DbDataSeeder
{
    private readonly WarehouseDbContext _context;

    public DbDataSeeder(WarehouseDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task SeedTestData()
    {
        if(!_context.Categories.Any())
            SeedTestCategories();
        
        if(!_context.Brands.Any())
            SeedTestBrands();
        
        if(!_context.Providers.Any())
            SeedTestProviders();
            
        if(!_context.Products.Any())
            SeedTestProducts();
        
        await _context.SaveChangesAsync();
    }

    private void SeedTestCategories()
    {
        _context.Categories.AddRange(EntitiesFixture.Categories);
    }
    
    private void SeedTestBrands()
    {
        _context.Brands.AddRange(EntitiesFixture.Brands);
    }

    private void SeedTestProviders()
    {
        _context.Providers.AddRange(EntitiesFixture.Providers);
    }

    private void SeedTestProducts()
    {
        _context.Products.AddRange(EntitiesFixture.Products);
    }
}