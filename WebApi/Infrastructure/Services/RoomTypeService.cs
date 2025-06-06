using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IPropertyRepository _propertyRepository;

    private const int MaxNameLength = 100;
    private const int CurrencyLength = 3;
    private const int MaxTextItemLength = 50;

    public RoomTypeService(
        IRoomTypeRepository roomTypeRepository,
        IPropertyRepository propertyRepository )
    {
        _roomTypeRepository = roomTypeRepository;
        _propertyRepository = propertyRepository;
    }

    public RoomType? GetRoomTypeById( Guid id )
    {
        if ( id == Guid.Empty )
            throw new ArgumentException( "Invalid ID" );

        return _roomTypeRepository.GetById( id );
    }

    public IEnumerable<RoomType> GetAllRoomTypes() => _roomTypeRepository.GetAll();

    public IEnumerable<RoomType> GetRoomTypeByPropertyId( Guid propertyId )
    {
        if ( propertyId == Guid.Empty )
            throw new ArgumentException( "Invalid property ID" );

        return _roomTypeRepository.GetByPropertyId( propertyId );
    }

    public void AddRoomType( RoomType roomType )
    {
        ArgumentNullException.ThrowIfNull( roomType );

        ValidateRoomTypeBasic( roomType );

        if ( _propertyRepository.GetById( roomType.PropertyId ) == null )
            throw new ValidationException( "Property not found" );

        roomType.Id = Guid.NewGuid();
        _roomTypeRepository.Add( roomType );
    }

    public void UpdateRoomType( RoomType roomType )
    {
        if ( roomType.Id == Guid.Empty )
            throw new ArgumentException( "Invalid room type" );

        ValidateRoomTypeBasic( roomType );

        _roomTypeRepository.Update( roomType );
    }

    public void DeleteRoomType( Guid id )
    {
        if ( id == Guid.Empty )
            throw new ArgumentException( "Invalid ID" );

        RoomType roomType = _roomTypeRepository.GetById( id )
                            ?? throw new KeyNotFoundException( "Room type not found" );

        _roomTypeRepository.Delete( roomType );
    }

    private static void ValidateRoomTypeBasic( RoomType roomType )
    {
        if ( string.IsNullOrWhiteSpace( roomType.Name ) || roomType.Name.Length > MaxNameLength )
            throw new ValidationException( $"Name must be 1-{MaxNameLength} chars" );

        if ( roomType.DailyPrice <= 0 )
            throw new ValidationException( "Price must be positive" );

        if ( string.IsNullOrWhiteSpace( roomType.Currency ) || roomType.Currency.Length != CurrencyLength )
            throw new ValidationException( $"Currency must be {CurrencyLength} chars" );

        if ( roomType.MinPersonCount <= 0 || roomType.MaxPersonCount <= 0 )
            throw new ValidationException( "Person count must be positive" );

        if ( roomType.MinPersonCount > roomType.MaxPersonCount )
            throw new ValidationException( "Min persons cannot exceed max" );

        if ( roomType.Services.Any( s => string.IsNullOrWhiteSpace( s ) || s.Length > MaxTextItemLength ) )
            throw new ValidationException( $"Services must be 1-{MaxTextItemLength} chars" );

        if ( roomType.Amenities.Any( a => string.IsNullOrWhiteSpace( a ) || a.Length > MaxTextItemLength ) )
            throw new ValidationException( $"Amenities must be 1-{MaxTextItemLength} chars" );
    }
}