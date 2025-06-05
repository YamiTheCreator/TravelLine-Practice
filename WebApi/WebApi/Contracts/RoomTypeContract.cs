namespace Web_Api.Contracts;

public class RoomTypeContract
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public string Currency { get; set; } = "USD";
    public int MinPersonCount { get; set; }
    public int MaxPersonCount { get; set; }
    public List<string> Services { get; set; } = [ ];
    public List<string> Amenities { get; set; } = [ ];
}