using Domain.Entities;

namespace Domain.Repositories;

public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllAsync();
    Task<Property?> GetByIdAsync(Guid id);
    Task<Property> AddAsync(Property property);
    Task<Property?> UpdateAsync(Property property);
    Task<bool> DeleteAsync(Guid id);
}