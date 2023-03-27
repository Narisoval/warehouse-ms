using Domain.Entities;
using FluentResults;

namespace Infrastructure.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    //TODO: pagination
    new Task<Result> Add(Product entity);
    
    new Task<Result<bool>> Update(Product productWithNewValues);
}