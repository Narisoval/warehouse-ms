using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.Generics;

namespace Infrastructure.Repositories;

public sealed class ProviderRepository : PaginatedRepository<Provider,WarehouseDbContext>,
    IProviderRepository
{
    public ProviderRepository(WarehouseDbContext context) : base(context)
    {
        ;
    }
}