using Domain.Entities;

namespace Domain.Services;

public interface IPropertyService
{
    Task<IEnumerable<Property>> GetAllPropertiesAsync();
    Task<Property?> GetPropertyByIdAsync(Guid id);
    Task<Property> AddPropertyAsync(Property property);
    Task<Property> UpdatePropertyAsync(Property property);
    Task<bool> DeletePropertyAsync(Guid id);
}