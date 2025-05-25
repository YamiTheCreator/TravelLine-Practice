using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(Guid id)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reservation>> GetFilteredAsync(
        Guid? propertyId,
        Guid? roomTypeId,
        DateTime? startDate,
        DateTime? endDate,
        string? guestName)
    {
        var query = _context.Reservations.AsQueryable();

        if (propertyId.HasValue)
            query = query.Where(r => r.PropertyId == propertyId);

        if (roomTypeId.HasValue)
            query = query.Where(r => r.RoomTypeId == roomTypeId);
        
        if (startDate.HasValue && endDate.HasValue)
            query = query.Where(r => 
                r.ArrivalDate < endDate && 
                r.DepartureDate > startDate);

        if (!string.IsNullOrEmpty(guestName))
            query = query.Where(r => r.GuestName.Contains(guestName));

        return await query.ToListAsync();
    }

    public async Task<Reservation> AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task<bool> UpdateAsync(Reservation reservation)
    {
        var existing = await _context.Reservations.FindAsync(reservation.Id);
        if (existing == null)
            return false;

        _context.Entry(existing).CurrentValues.SetValues(reservation);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CancelAsync(Guid id)
    {
        var reservation = await GetByIdAsync(id);
        if (reservation == null || reservation.IsCancelled)
            return false;

        reservation.IsCancelled = true;
        return await UpdateAsync(reservation);
    }
}