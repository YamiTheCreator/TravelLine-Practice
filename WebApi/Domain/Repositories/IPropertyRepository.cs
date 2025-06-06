using Domain.Entities;

namespace Domain.Repositories;

public interface IPropertyRepository
{
    void Add( Property property );
    void Update( Property property );
    void Delete( Property property );
    Property? GetById( Guid id );
    IEnumerable<Property> GetAll();
}