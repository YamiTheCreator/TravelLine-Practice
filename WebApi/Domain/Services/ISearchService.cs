using Domain.Entities;

namespace Domain.Services;

public interface ISearchService
{
    IEnumerable<(Property Property, RoomType RoomType)> SearchAvailableRooms(
        string city,
        DateTime arrivalDate,
        DateTime departureDate,
        int personCount,
        decimal? maxPrice = null );
}