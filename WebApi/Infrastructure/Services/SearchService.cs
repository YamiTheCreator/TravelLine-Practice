using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services;

public class SearchService : ISearchService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IReservationRepository _reservationRepository;

    public SearchService(
        IPropertyRepository propertyRepository,
        IRoomTypeRepository roomTypeRepository,
        IReservationRepository reservationRepository )
    {
        _propertyRepository = propertyRepository;
        _roomTypeRepository = roomTypeRepository;
        _reservationRepository = reservationRepository;
    }

    public IEnumerable<(Property Property, RoomType RoomType)> SearchAvailableRooms(
        string city,
        DateTime arrivalDate,
        DateTime departureDate,
        int personCount,
        decimal? maxPrice = null )
    {
        if ( string.IsNullOrWhiteSpace( city ) )
            throw new ArgumentException( "City must be specified" );

        if ( departureDate <= arrivalDate )
            throw new ArgumentException( "Departure date must be after arrival date" );

        if ( personCount <= 0 )
            throw new ArgumentException( "Person count must be positive" );

        List<Property> properties = _propertyRepository.GetAll()
            .Where( p => p.City.Equals( city.Trim(), StringComparison.OrdinalIgnoreCase ) )
            .ToList();

        if ( properties.Count == 0 )
            return [ ];

        List<RoomType> allRoomTypes = _roomTypeRepository.GetAll()
            .Where( rt => properties.Select( p => p.Id ).Contains( rt.PropertyId ) )
            .ToList();

        List<RoomType> suitableRoomTypes = allRoomTypes
            .Where( rt => rt.MaxPersonCount >= personCount &&
                          rt.MinPersonCount <= personCount &&
                          ( maxPrice == null || rt.DailyPrice <= maxPrice ) )
            .ToList();

        if ( suitableRoomTypes.Count == 0 )
            return [ ];

        List<Guid> reservedRoomTypeIds = _reservationRepository.GetAll()
            .Where( r => !r.IsCancelled &&
                         r.DepartureDate > arrivalDate &&
                         r.ArrivalDate < departureDate &&
                         suitableRoomTypes.Select( rt => rt.Id ).Contains( r.RoomTypeId ) )
            .Select( r => r.RoomTypeId )
            .Distinct()
            .ToList();

        List<RoomType> availableRoomTypes = suitableRoomTypes
            .Where( rt => !reservedRoomTypeIds.Contains( rt.Id ) )
            .ToList();

        List<(Property Property, RoomType RoomType)> result = availableRoomTypes
            .Join( properties,
                roomType => roomType.PropertyId,
                property => property.Id,
                ( roomType, property ) => ( Property: property, RoomType: roomType ) )
            .OrderBy( x => x.RoomType.DailyPrice )
            .ThenBy( x => x.Property.Name )
            .ToList();

        return result;
    }
}