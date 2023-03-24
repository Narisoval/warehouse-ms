using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public sealed class BrandRepository : Repository<Brand,WarehouseDbContext>,IBrandRepository
{
    public BrandRepository(WarehouseDbContext context) : base(context)
    {
    }

}