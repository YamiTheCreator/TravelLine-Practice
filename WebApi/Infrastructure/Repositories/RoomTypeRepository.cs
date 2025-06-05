using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly ApplicationDbContext _context;

    public RoomTypeRepository( ApplicationDbContext context )
    {
        _context = context;
    }

    public void Add( RoomType roomType )
    {
        _context.RoomTypes.Add( roomType );
        _context.SaveChanges();
    }

    public void Update( RoomType roomType )
    {
        _context.RoomTypes.Update( roomType );
        _context.SaveChanges();
    }

    public void Delete( RoomType roomType )
    {
        _context.RoomTypes.Remove( roomType );
        _context.SaveChanges();
    }

    public RoomType? GetById( Guid id )
    {
        return _context.RoomTypes
            .FirstOrDefault( rt => rt.Id == id );
    }

    public IEnumerable<RoomType> GetByPropertyId( Guid propertyId )
    {
        return _context.RoomTypes
            .Where( rt => rt.PropertyId == propertyId )
            .ToList();
    }

    public IEnumerable<RoomType> GetAll()
    {
        return _context.RoomTypes
            .ToList();
    }
}