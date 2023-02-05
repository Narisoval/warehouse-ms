using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class ProductRepository : Repository<Product,WarehouseDbContext>, IProductRepository
{
    
    public ProductRepository(WarehouseDbContext context) : base(context)
    {
        ;
    }

    // The role of this method is to ensure that product
    // with non existing brand, provider or category can't be added. 
    public new async Task Add(Product entity)
    {
        await CheckBrandExists(entity.BrandId);
        await CheckProviderExists(entity.ProviderId);
        await CheckCategoryExists(entity.CategoryId);
        
        await base.Add(entity);
    }

    public new async Task<Product?> Get(Guid id)
    {
        return await Context.Products.AsNoTracking()
            .Include(product => product.Brand)
            .Include(product =>product.Images)
            .Include(product => product.Category)
            .Where(product => product.Id == id)
            .FirstOrDefaultAsync();
    }
    
    public new async Task<IEnumerable<Product>> GetAll()
    {
        return await Context.Products.AsNoTracking()
            .Include(product => product.Brand)
            .Include(product => product.Images)
            .Include(product => product.Category)
            .ToListAsync();
    }

    public async Task<Product?> GetProductWithProvider(Guid id)
    {
        return await Context.Products.AsNoTracking()
            .Include(product => product.Brand)
            .Include(product => product.Images)
            .Include(product => product.Category)
            .Include(product => product.Provider)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetAllProductsWithProvider()
    {
        return await Context.Products.AsNoTracking()
            .Include(product => product.Brand)
            .Include(product => product.Images)
            .Include(product => product.Category)
            .Include(product => product.Provider)
            .ToListAsync();
    }

    private async Task CheckBrandExists(Guid id)
    {
        bool brandExists = await Context.Brands.AnyAsync(brand => brand.Id == id);
        if (!brandExists)
        {
            throw new InvalidOperationException("Product's brand does not exist.");
        }
    }
    
    private async Task CheckProviderExists(Guid id)
    {
        bool providerExists = await Context.Providers.AnyAsync(provider => provider.Id == id);
        if (!providerExists)
        {
            throw new InvalidOperationException("Product's provider does not exist.");
        }
    }

    private async Task CheckCategoryExists(Guid id)
    {
        bool categoryExists = await Context.Categories.AnyAsync(provider => provider.Id == id);
        if (!categoryExists)
        {
            throw new InvalidOperationException("Product's category does not exist.");
        }
    }
}