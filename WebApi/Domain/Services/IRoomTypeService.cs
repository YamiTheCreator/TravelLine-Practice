using Domain.Entities;

namespace Domain.Services;

public interface IRoomTypeService
{
    RoomType? GetRoomTypeById( Guid id );
    IEnumerable<RoomType> GetAllRoomTypes();

    IEnumerable<RoomType> GetRoomTypeByPropertyId( Guid propertyId );
    void AddRoomType( RoomType roomType );
    void UpdateRoomType( RoomType roomType );
    void DeleteRoomType( Guid id );
}