using Domain.Entities;
using FluentResults;
using Infrastructure.Interfaces.Generics;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IPaginatedRepository<Product>
{
    //TODO: pagination
    new Task<Result> Add(Product entity);
    
    new Task<Result<bool>> Update(Product productWithNewValues);
}