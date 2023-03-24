using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public sealed class ProviderRepository : Repository<Provider,WarehouseDbContext>,
    IProviderRepository
{
    public ProviderRepository(WarehouseDbContext context) : base(context)
    {
        ;
    }
}