namespace Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public TimeSpan ArrivalTime { get; set; } = TimeSpan.FromHours( 14 );
    public TimeSpan DepartureTime { get; set; } = TimeSpan.FromHours( 12 );
    public string GuestName { get; set; }
    public string GuestPhoneNumber { get; set; }
    public decimal Total { get; set; }
    public string Currency { get; set; }
    public bool IsCancelled { get; set; }
}