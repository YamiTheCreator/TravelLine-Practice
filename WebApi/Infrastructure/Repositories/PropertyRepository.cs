using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDbContext _context;

        public PropertyRepository( ApplicationDbContext context )
        {
            _context = context;
        }

        public void Add( Property property )
        {
            _context.Properties.Add( property );
            _context.SaveChanges();
        }

        public void Update( Property property )
        {
            _context.Properties.Update( property );
            _context.SaveChanges();
        }

        public void Delete( Property property )
        {
            _context.Properties.Remove( property );
            _context.SaveChanges();
        }

        public Property? GetById( Guid id )
        {
            return _context.Properties
                .Include( p => p.RoomTypes )
                .FirstOrDefault( p => p.Id == id );
        }

        public IEnumerable<Property> GetAll()
        {
            return _context.Properties
                .Include( p => p.RoomTypes )
                .ToList();
        }
    }
}