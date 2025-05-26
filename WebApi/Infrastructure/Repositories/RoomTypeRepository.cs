using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomType>> GetByPropertyIdAsync(Guid propertyId)
        {
            return await _context.RoomTypes
                .Where(rt => rt.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task<RoomType?> GetByIdAsync(Guid id)
        {
            return await _context.RoomTypes
                .FirstOrDefaultAsync(rt => rt.Id == id);
        }

        public async Task<RoomType> AddAsync(RoomType roomType)
        {
            await _context.RoomTypes.AddAsync(roomType);
            await _context.SaveChangesAsync();
            return roomType;
        }

        public async Task<RoomType?> UpdateAsync(RoomType roomType)
        {
            var existing = await _context.RoomTypes.FindAsync(roomType.Id);
            if (existing == null)
                return null;

            _context.Entry(existing).CurrentValues.SetValues(roomType);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.RoomTypes.FindAsync(id);
            if (existing == null)
                return false;

            _context.RoomTypes.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.RoomTypes
                .AnyAsync(rt => rt.Id == id);
        }
    }
}