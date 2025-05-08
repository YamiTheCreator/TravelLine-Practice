using Accommodations.Models;

namespace Accommodations
{
    public interface IBookingService
    {
        Booking? Book( int userId, string categoryName, DateTime startDate, DateTime endDate, Currency currency );
        void CancelBooking( Guid bookingId );

        Booking? FindBookingById( Guid bookingId );

        //убрал ? потому то в случае если записей нет - вернется пустой список
        IEnumerable<Booking> SearchBookings( DateTime startDate, DateTime endDate, string categoryName );
        decimal CalculateCancellationPenaltyAmount( Booking booking );
    }
}