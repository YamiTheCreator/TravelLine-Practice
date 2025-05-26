using Domain.Entities;

namespace Domain.Services;

public interface IRoomTypeService
{
    Task<IEnumerable<RoomType>> GetRoomTypesByPropertyIdAsync(Guid propertyId);
    Task<RoomType?> GetRoomTypeByIdAsync(Guid id);

    Task<RoomType> CreateRoomTypeForPropertyAsync(
        Guid propertyId,
        RoomType roomType);

    Task<RoomType> UpdateRoomTypeAsync(RoomType roomType);
    Task<bool> DeleteRoomTypeAsync(Guid id);
}