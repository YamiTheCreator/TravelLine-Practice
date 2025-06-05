using Domain.Entities;
using Domain.Repositories;

namespace Domain.Services;

public interface IReservationService
{
    Reservation CreateReservation(
        Guid propertyId,
        Guid roomTypeId,
        DateTime arrivalDate,
        DateTime departureDate,
        int personCount,
        string guestName,
        string guestPhoneNumber );

    void CancelReservation( Guid reservationId, bool softDelete = true );
    Reservation? GetReservation( Guid id );

    IEnumerable<Reservation> GetReservations(
        Guid? propertyId = null,
        Guid? roomTypeId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        string? guestName = null );
}