using Domain.Entities;

namespace Domain.Repositories;

public interface IRoomTypeRepository
{
    Task<IEnumerable<RoomType>> GetByPropertyIdAsync(Guid propertyId);
    Task<RoomType?> GetByIdAsync(Guid id);
    Task<RoomType> AddAsync(RoomType roomType);
    Task<RoomType?> UpdateAsync(RoomType roomType);
    Task<bool> DeleteAsync(Guid id);
}