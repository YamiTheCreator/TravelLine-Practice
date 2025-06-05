using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IPropertyRepository _propertyRepository;

    public ReservationService(
        IReservationRepository reservationRepository,
        IRoomTypeRepository roomTypeRepository,
        IPropertyRepository propertyRepository )
    {
        _reservationRepository = reservationRepository;
        _roomTypeRepository = roomTypeRepository;
        _propertyRepository = propertyRepository;
    }

    public Reservation CreateReservation(
        Guid propertyId,
        Guid roomTypeId,
        DateTime arrivalDate,
        DateTime departureDate,
        int personCount,
        string guestName,
        string guestPhoneNumber )
    {
        ValidateReservationParameters( arrivalDate, departureDate, personCount, guestName, guestPhoneNumber );

        RoomType roomType = _roomTypeRepository.GetById( roomTypeId )
                            ?? throw new KeyNotFoundException( "Room type not found" );

        if ( personCount < roomType.MinPersonCount || personCount > roomType.MaxPersonCount )
            throw new ValidationException(
                $"Person count must be between {roomType.MinPersonCount} and {roomType.MaxPersonCount}" );

        IEnumerable<Reservation> activeReservations = _reservationRepository
            .GetAllReservations( roomTypeId, arrivalDate, departureDate );

        if ( activeReservations.Any() )
            throw new ValidationException( "Room is not available for selected dates" );

        int nights = ( departureDate - arrivalDate ).Days;
        decimal total = roomType.DailyPrice * nights;

        Reservation reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            PropertyId = propertyId,
            RoomTypeId = roomTypeId,
            ArrivalDate = arrivalDate,
            DepartureDate = departureDate,
            GuestName = guestName,
            GuestPhoneNumber = guestPhoneNumber,
            Total = total,
            Currency = roomType.Currency,
            IsCancelled = false
        };

        _reservationRepository.Add( reservation );
        return reservation;
    }

    public void CancelReservation( Guid reservationId, bool softDelete = true )
    {
        Reservation reservation = _reservationRepository.GetById( reservationId )
                                  ?? throw new KeyNotFoundException( "Reservation not found" );

        if ( softDelete )
        {
            reservation.IsCancelled = true;
            _reservationRepository.Update( reservation );
        }
        else
        {
            _reservationRepository.Delete( reservation );
        }
    }

    public Reservation? GetReservation( Guid id )
    {
        return _reservationRepository.GetById( id );
    }

    public IEnumerable<Reservation> GetReservations(
        Guid? propertyId = null,
        Guid? roomTypeId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? guestName = null )
    {
        IEnumerable<Reservation> query = _reservationRepository.GetAll();

        if ( propertyId.HasValue )
            query = query.Where( r => r.PropertyId == propertyId.Value );

        if ( roomTypeId.HasValue )
            query = query.Where( r => r.RoomTypeId == roomTypeId.Value );

        if ( fromDate.HasValue )
            query = query.Where( r => r.DepartureDate >= fromDate.Value );

        if ( toDate.HasValue )
            query = query.Where( r => r.ArrivalDate <= toDate.Value );

        if ( !string.IsNullOrEmpty( guestName ) )
            query = query.Where( r => r.GuestName.Contains( guestName ) );

        return query.ToList();
    }

    private static void ValidateReservationParameters(
        DateTime arrivalDate,
        DateTime departureDate,
        int personCount,
        string guestName,
        string guestPhoneNumber )
    {
        if ( departureDate <= arrivalDate )
            throw new ValidationException( "Departure date must be after arrival date" );

        if ( arrivalDate < DateTime.Today )
            throw new ValidationException( "Arrival date cannot be in the past" );

        if ( personCount <= 0 )
            throw new ValidationException( "Person count must be positive" );

        if ( string.IsNullOrWhiteSpace( guestName ) )
            throw new ValidationException( "Guest name is required" );

        if ( string.IsNullOrWhiteSpace( guestPhoneNumber ) )
            throw new ValidationException( "Guest phone number is required" );
    }
}