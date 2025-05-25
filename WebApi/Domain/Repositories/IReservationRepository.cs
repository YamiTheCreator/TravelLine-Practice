using Domain.Entities;

namespace Domain.Repositories;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(Guid id);

    Task<IEnumerable<Reservation>> GetFilteredAsync(
        Guid? propertyId,
        Guid? roomTypeId,
        DateTime? startDate,
        DateTime? endDate,
        string? guestName);

    Task<Reservation> AddAsync(Reservation reservation);
    Task<bool> UpdateAsync(Reservation reservation);
    Task<bool> CancelAsync(Guid id);
}