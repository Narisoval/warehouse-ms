using Domain.Entities;
using Infrastructure.Interfaces.Generics;

namespace Infrastructure.Interfaces;

public interface IProviderRepository : IPaginatedRepository<Provider>
{
}