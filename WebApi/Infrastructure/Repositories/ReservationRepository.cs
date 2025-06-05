using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository( ApplicationDbContext context )
    {
        _context = context;
    }

    public void Add( Reservation reservation )
    {
        _context.Reservations.Add( reservation );
        _context.SaveChanges();
    }

    public void Update( Reservation reservation )
    {
        _context.Reservations.Update( reservation );
        _context.SaveChanges();
    }

    public void Delete( Reservation reservation )
    {
        _context.Reservations.Remove( reservation );
        _context.SaveChanges();
    }

    public Reservation? GetById( Guid id )
    {
        return _context.Reservations
            .Include( r => r.PropertyId )
            .Include( r => r.RoomTypeId )
            .FirstOrDefault( r => r.Id == id );
    }

    public IEnumerable<Reservation> GetByPropertyId( Guid propertyId )
    {
        return _context.Reservations
            .Where( r => r.PropertyId == propertyId )
            .ToList();
    }

    public IEnumerable<Reservation> GetByRoomTypeId( Guid roomTypeId )
    {
        return _context.Reservations
            .Where( r => r.RoomTypeId == roomTypeId )
            .ToList();
    }

    public IEnumerable<Reservation> GetAllReservations( Guid roomTypeId, DateTime from, DateTime to )
    {
        return _context.Reservations
            .Where( r => r.RoomTypeId == roomTypeId &&
                         !r.IsCancelled &&
                         r.DepartureDate > from &&
                         r.ArrivalDate < to )
            .ToList();
    }

    public IEnumerable<Reservation> GetAll()
    {
        return _context.Reservations.ToList();
    }
}