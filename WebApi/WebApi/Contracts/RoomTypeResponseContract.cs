namespace WebApi.Contracts;

public class RoomTypeResponseContract
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public string Name { get; set; } = null!;
    public decimal DailyPrice { get; set; }
    public string Currency { get; set; } = null!;
    public int MinPersonCount { get; set; }
    public int MaxPersonCount { get; set; }
    public List<string> Services { get; set; } = [];
    public List<string> Amenities { get; set; } = [];
}