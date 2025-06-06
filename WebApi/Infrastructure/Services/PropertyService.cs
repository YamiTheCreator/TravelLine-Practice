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

        public PropertyService( IPropertyRepository propertyRepository )
        {
            _propertyRepository = propertyRepository;
        }

        public IEnumerable<Property> GetAllProperties()
        {
            return _propertyRepository.GetAll();
        }

        public Property? GetPropertyById( Guid id )
        {
            if ( id == Guid.Empty )
                throw new ValidationException( "Property Id must be specified." );

            return _propertyRepository.GetById( id );
        }

        public void AddProperty( Property property )
        {
            if ( property.Id == Guid.Empty )
                property.Id = Guid.NewGuid();

            ValidateProperty( property );
            _propertyRepository.Add( property );
        }

        public void UpdateProperty( Property property )
        {
            if ( property.Id == Guid.Empty )
                throw new ValidationException( "Property Id must be specified." );

            ValidateProperty( property );
            _propertyRepository.Update( property );
        }

        public void DeleteProperty( Guid id )
        {
            if ( id == Guid.Empty )
                throw new ValidationException( "Property Id must be specified." );

            Property? property = _propertyRepository.GetById( id );
            if ( property == null )
                throw new KeyNotFoundException( $"Property with Id {id} not found." );

            _propertyRepository.Delete( property );
        }

        private static void ValidateProperty( Property property )
        {
            ArgumentNullException.ThrowIfNull( property );

            if ( string.IsNullOrWhiteSpace( property.Name ) || property.Name.Length > MaxNameLength )
                throw new ValidationException( $"Name is required and must be at most {MaxNameLength} characters." );
            if ( string.IsNullOrWhiteSpace( property.Country ) || property.Country.Length > MaxCountryLength )
                throw new ValidationException(
                    $"Country is required and must be at most {MaxCountryLength} characters." );
            if ( string.IsNullOrWhiteSpace( property.City ) || property.City.Length > MaxCityLength )
                throw new ValidationException( $"City is required and must be at most {MaxCityLength} characters." );
            if ( string.IsNullOrWhiteSpace( property.Address ) || property.Address.Length > MaxAddressLength )
                throw new ValidationException(
                    $"Address is required and must be at most {MaxAddressLength} characters." );

            if ( property.Latitude < -90 || property.Latitude > 90 )
                throw new ValidationException( "Latitude must be between -90 and 90." );
            if ( property.Longitude < -180 || property.Longitude > 180 )
                throw new ValidationException( "Longitude must be between -180 and 180." );
        }
    }
}