using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private const int MaxNameLength = 200;
        private const int MaxCountryLength = 100;
        private const int MaxCityLength = 100;
        private const int MaxAddressLength = 500;

        public PropertyService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {
            return await _propertyRepository.GetAllAsync();
        }

        public async Task<Property?> GetPropertyByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Property Id cannot be empty.");

            return await _propertyRepository.GetByIdAsync(id);
        }

        public async Task<Property> AddPropertyAsync(Property property)
        {
            ValidateProperty(property, isNew: true);

            property.Id = Guid.NewGuid();
            return await _propertyRepository.AddAsync(property);
        }

        public async Task<Property> UpdatePropertyAsync(Property property)
        {
            if (property.Id == Guid.Empty)
                throw new ValidationException("Property Id cannot be empty.");

            ValidateProperty(property, isNew: false);

            var updated = await _propertyRepository.UpdateAsync(property);
            if (updated == null)
                throw new KeyNotFoundException($"Property with Id {property.Id} not found.");

            return updated;
        }

        public async Task<bool> DeletePropertyAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Property Id cannot be empty.");

            return await _propertyRepository.DeleteAsync(id);
        }

        private void ValidateProperty(Property property, bool isNew)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            
            if (string.IsNullOrWhiteSpace(property.Name) || property.Name.Length > MaxNameLength)
                throw new ValidationException($"Name is required and must be at most {MaxNameLength} characters.");
            if (string.IsNullOrWhiteSpace(property.Country) || property.Country.Length > MaxCountryLength)
                throw new ValidationException($"Country is required and must be at most {MaxCountryLength} characters.");
            if (string.IsNullOrWhiteSpace(property.City) || property.City.Length > MaxCityLength)
                throw new ValidationException($"City is required and must be at most {MaxCityLength} characters.");
            if (string.IsNullOrWhiteSpace(property.Address) || property.Address.Length > MaxAddressLength)
                throw new ValidationException($"Address is required and must be at most {MaxAddressLength} characters.");
            
            if (property.Latitude < -90 || property.Latitude > 90)
                throw new ValidationException("Latitude must be between -90 and 90.");
            if (property.Longitude < -180 || property.Longitude > 180)
                throw new ValidationException("Longitude must be between -180 and 180.");
            
            var duplicates = _propertyRepository.GetAllAsync().Result
                .Any(p => string.Equals(p.Name, property.Name, StringComparison.OrdinalIgnoreCase)
                          && string.Equals(p.City, property.City, StringComparison.OrdinalIgnoreCase)
                          && (isNew || p.Id != property.Id));
            if (duplicates)
                throw new ValidationException("A property with the same name already exists in this city.");
        }
    }
}
