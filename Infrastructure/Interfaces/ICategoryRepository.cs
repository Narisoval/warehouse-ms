using Domain.Entities;
using Infrastructure.Interfaces.Generics;

namespace Infrastructure.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{ 
    Task<IEnumerable<Category>> GetAll();
}