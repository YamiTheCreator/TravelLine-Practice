using Domain.Entities;

namespace Domain.Repositories;

public interface IRoomTypeRepository
{
    void Add( RoomType roomType );
    void Update( RoomType roomType );
    void Delete( RoomType roomType );
    RoomType? GetById( Guid id );
    IEnumerable<RoomType> GetByPropertyId( Guid propertyId );
    IEnumerable<RoomType> GetAll();
}