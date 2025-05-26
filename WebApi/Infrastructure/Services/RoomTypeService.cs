using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IPropertyRepository _propertyRepository;

    public RoomTypeService(
        IRoomTypeRepository roomTypeRepository,
        IPropertyRepository propertyRepository)
    {
        _roomTypeRepository = roomTypeRepository;
        _propertyRepository = propertyRepository;
    }

    public async Task<RoomType> CreateRoomTypeForPropertyAsync(
        Guid propertyId,
        RoomType roomType)
    {
        ArgumentNullException.ThrowIfNull(roomType);

        var property = await _propertyRepository.GetByIdAsync(propertyId);
        if (property == null)
            throw new ArgumentException($"Property with Id {propertyId} not found.");

        if (roomType.MinPersonCount > roomType.MaxPersonCount)
            throw new ArgumentException("MinPersonCount cannot be greater than MaxPersonCount.");

        roomType.Id = Guid.NewGuid();
        roomType.PropertyId = propertyId;

        return await _roomTypeRepository.AddAsync(roomType);
    }

    public async Task<IEnumerable<RoomType>> GetRoomTypesByPropertyIdAsync(Guid propertyId)
    {
        return await _roomTypeRepository.GetByPropertyIdAsync(propertyId);
    }

    public async Task<RoomType?> GetRoomTypeByIdAsync(Guid id)
    {
        return await _roomTypeRepository.GetByIdAsync(id);
    }

    public async Task<RoomType> UpdateRoomTypeAsync(RoomType roomType)
    {
        ArgumentNullException.ThrowIfNull(roomType);

        if (roomType.MinPersonCount > roomType.MaxPersonCount)
            throw new ArgumentException("MinPersonCount cannot be greater than MaxPersonCount.");

        var updated = await _roomTypeRepository.UpdateAsync(roomType);
        if (updated == null)
            throw new KeyNotFoundException($"RoomType with Id {roomType.Id} not found.");

        return updated;
    }

    public async Task<bool> DeleteRoomTypeAsync(Guid id)
    {
        return await _roomTypeRepository.DeleteAsync(id);
    }
}