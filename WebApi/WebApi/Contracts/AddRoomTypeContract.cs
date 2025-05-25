using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts;

public class AddRoomTypeContract
{
    [Required(ErrorMessage = "PropertyId is required")]
    public Guid PropertyId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = null!;

    [Range(0.01, double.MaxValue, ErrorMessage = "DailyPrice must be greater than 0")]
    public decimal DailyPrice { get; set; }

    [Required(ErrorMessage = "Currency is required")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be 3 characters")]
    public string Currency { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "MinPersonCount must be at least 1")]
    public int MinPersonCount { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "MaxPersonCount must be at least 1")]
    public int MaxPersonCount { get; set; }

    public List<string> Services { get; set; } = new();
    public List<string> Amenities { get; set; } = new();
}