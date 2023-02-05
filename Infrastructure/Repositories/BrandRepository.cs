using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BrandRepository : Repository<Brand>,IBrandRepository
{
    private WarehouseDbContext WarehouseContext => Context as WarehouseDbContext;
    
    public BrandRepository(DbContext context) : base(context)
    {
    }

    public async Task<Brand?> GetBrandWithProducts(Guid id)
    {
        return await WarehouseContext.Brands
            .Include(brand => brand.Products)
            .Where(brand => brand.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Brand>> GetAllBrandsWithProducts()
    {
        return await WarehouseContext.Brands.AsNoTracking()
            .Include(brand => brand.Products)
            .ToListAsync();
    }
}