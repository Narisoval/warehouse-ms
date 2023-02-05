using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : Repository<Product,WarehouseDbContext>, IProductRepository
{
    
    public ProductRepository(WarehouseDbContext context) : base(context)
    {
    }

    // The role of this method is to ensure that when product is added to the database
    // nothing more than a product and its images is added or changed. 
    public new async Task Add(Product entity)
    {
        await CheckIfBrandExists(entity.BrandId);
        await CheckIfProviderExists(entity.CategoryId);
        await CheckIfCategoryExists(entity.CategoryId);

        entity.ChangeBrand(null);
        entity.ChangeProvider(null);
        entity.ChangeCategory(null);

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

    private async Task CheckIfBrandExists(Guid id)
    {
        bool brandExists = await Context.Brands.AnyAsync(brand => brand.Id == id);
        if (!brandExists)
        {
            throw new InvalidOperationException("Product's brand does not exist.");
        }
    }
    
    private async Task CheckIfProviderExists(Guid id)
    {
        bool providerExists = await Context.Providers.AnyAsync(provider => provider.Id == id);
        if (!providerExists)
        {
            throw new InvalidOperationException("Product's provider does not exist.");
        }
    }

    private async Task CheckIfCategoryExists(Guid id)
    {
        bool providerExists = await Context.Providers.AnyAsync(provider => provider.Id == id);
        if (!providerExists)
        {
            throw new InvalidOperationException("Product's category does not exist.");
        }
    }
}