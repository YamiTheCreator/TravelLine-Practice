namespace Web_Api.Contracts;

public class AvailableRoomResultContract
{
    public PropertyContract Property { get; set; }
    public RoomTypeContract RoomType { get; set; }
    public decimal TotalPrice { get; set; }
}