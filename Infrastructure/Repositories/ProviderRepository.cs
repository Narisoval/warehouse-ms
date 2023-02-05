using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class ProviderRepository : Repository<Provider,WarehouseDbContext>,
    IProviderRepository
{
    public ProviderRepository(WarehouseDbContext context) : base(context)
    {
        ;
    }

    public async Task<Provider?> GetProviderWithProducts(Guid id)
    {
        return await Context.Providers
            .AsNoTracking()
            .Include(provider => provider.Products)
            .Where(provider => provider.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Provider>> GetAllProvidersWithProducts()
    {
        return await Context.Providers
            .AsNoTracking()
            .Include(provider => provider.Products)
            .ToListAsync();
    }
}