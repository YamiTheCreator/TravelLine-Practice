using Domain.Entities;

namespace Domain.Services;

public interface IReservationService
{
    Task<Reservation?> GetReservationByIdAsync(Guid id);

    Task<IEnumerable<Reservation>> GetReservationsAsync(
        Guid? propertyId = null,
        Guid? roomTypeId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? guestName = null);

    Task<Reservation> CreateReservationAsync(Reservation reservation);
    Task<bool> CancelReservationAsync(Guid id);
    Task<bool> IsRoomTypeAvailableAsync(Guid roomTypeId, DateTime arrivalDate, DateTime departureDate);
}