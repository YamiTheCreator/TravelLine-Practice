using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Services;

public interface IPropertyService
{
    IEnumerable<Property> GetAllProperties();
    Property? GetPropertyById( Guid id );
    void AddProperty( Property property );
    void UpdateProperty( Property property );
    void DeleteProperty( Guid id );
}