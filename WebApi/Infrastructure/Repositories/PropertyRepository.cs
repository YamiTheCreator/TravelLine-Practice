using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly ApplicationDbContext _context;

    public PropertyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _context.Properties
            .Include(p => p.RoomTypes)
            .ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(Guid id)
    {
        return await _context.Properties
            .Include(p => p.RoomTypes)
            .FirstOrDefaultAsync(p => Equals(p.Id, id));
    }

    public async Task<Property> AddAsync(Property property)
    {
        await _context.Properties.AddAsync(property);
        await _context.SaveChangesAsync();
        return property;
    }

    public async Task<Property?> UpdateAsync(Property property)
    {
        var existingProperty = await _context.Properties.FindAsync(property.Id);
        if (existingProperty == null) return null;

        _context.Entry(existingProperty).CurrentValues.SetValues(property);
        await _context.SaveChangesAsync();
        return existingProperty;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var property = await _context.Properties.FindAsync(id);
        if (property == null) return false;

        _context.Properties.Remove(property);
        await _context.SaveChangesAsync();
        return true;
    }
}