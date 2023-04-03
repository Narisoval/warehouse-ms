using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories.Generics;

namespace Infrastructure.Repositories;

public sealed class BrandRepository : PaginatedRepository<Brand,WarehouseDbContext>,IBrandRepository
{
    public BrandRepository(WarehouseDbContext context) : base(context)
    {
    }

}