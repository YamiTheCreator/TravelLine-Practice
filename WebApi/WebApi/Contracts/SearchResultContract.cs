namespace WebApi.Contracts;

public class SearchResultContract
{
    public PropertySearchResult Property { get; set; } = null!;
    public RoomTypeSearchResult RoomType { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = null!;
    public int Nights { get; set; }
}

public class PropertySearchResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Address { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class RoomTypeSearchResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal DailyPrice { get; set; }
    public string Currency { get; set; } = null!;
    public int MinPersonCount { get; set; }
    public int MaxPersonCount { get; set; }
    public List<string> Services { get; set; } = [];
    public List<string> Amenities { get; set; } = [];
}