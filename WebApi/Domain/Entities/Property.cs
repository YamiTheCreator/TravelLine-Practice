namespace Domain.Entities;
public class Property
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    public ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
}
