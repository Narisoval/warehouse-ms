using Domain.Entities;
using FluentResults;
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

    // The goal of this method is to return an error when a product
    // with non existing brand, provider or category is being added 
    public new async Task<Result> Add(Product entity)
    {
        var result = await CheckForeignKeys(entity);
        
        if (result.IsSuccess)
            await base.Add(entity);
        
        return result;
    }
    
    public new async Task<Result<bool>> Update(Product productWithNewValues)
    {
        var result = await CheckForeignKeys(productWithNewValues);

        if (result.IsFailed)
            return result;
        
        var productFromDb = await Context.Products
            .Include(product => product.Images)
            .FirstOrDefaultAsync(product => product.Id == productWithNewValues.Id);
            
        
        if (productFromDb == null)
            return false;
        
        Context.Products.Entry(productFromDb).CurrentValues.SetValues(productWithNewValues);
        productFromDb.SetProductImages(productWithNewValues.Images); 
        return true;
    }

    public new async Task<Product?> Get(Guid id)
    {
        return await Context.Products
            .AsNoTracking()
            .Include(product => product.Brand)
            .Include(product => product.Images)
            .Include(product => product.Category)
            .Include(product => product.Provider)
            .Where(product => product.Id == id)
            .FirstOrDefaultAsync();
    }
    
    public new async Task<IEnumerable<Product>> GetAll(int pageIndex = 1,int pageSize = 15)
    {
        return await Context.Products.AsNoTracking()
            .Include(product => product.Brand)
            .Include(product => product.Images)
            .Include(product => product.Category)
            .Include(product => product.Provider)
            .Skip((pageIndex-1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    private async Task<Result> CheckForeignKeys(Product entity)
    {
        var result = new Result(); 
        await CheckBrandExists(entity.BrandId,result);
        await CheckProviderExists(entity.ProviderId,result);
        await CheckCategoryExists(entity.CategoryId,result);
        return result;
    }
    
    private async Task CheckBrandExists(Guid id, Result result)
    {
        bool brandExists = await Context.Brands.AnyAsync(brand => brand.Id == id);
        if (!brandExists)
            result.WithError("Product's brand does not exist.");
    }
    
    private async Task CheckProviderExists(Guid id, Result result)
    {
        bool providerExists = await Context.Providers.AnyAsync(provider => provider.Id == id);
        if (!providerExists)
            result.WithError("Product's provider does not exist.");
    }

    private async Task CheckCategoryExists(Guid id, Result result)
    {
        bool categoryExists = await Context.Categories.AnyAsync(provider => provider.Id == id);
        if (!categoryExists)
            result.WithError("Product's category does not exist.");
    }
}