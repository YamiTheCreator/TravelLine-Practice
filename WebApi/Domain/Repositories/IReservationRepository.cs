using Domain.Entities;

namespace Domain.Repositories;

public interface IReservationRepository
{
    void Add( Reservation reservation );
    void Update( Reservation reservation );
    void Delete( Reservation reservation );
    Reservation? GetById( Guid id );
    IEnumerable<Reservation> GetByPropertyId( Guid propertyId );
    IEnumerable<Reservation> GetByRoomTypeId( Guid roomTypeId );
    IEnumerable<Reservation> GetAllReservations( Guid roomTypeId, DateTime from, DateTime to );
    IEnumerable<Reservation> GetAll();
}