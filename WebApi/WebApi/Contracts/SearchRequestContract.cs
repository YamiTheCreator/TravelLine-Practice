using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts;

public class SearchRequestContract
{
    [Required(ErrorMessage = "City is required")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "ArrivalDate is required")]
    [DataType(DataType.Date)]
    public DateTime ArrivalDate { get; set; }

    [Required(ErrorMessage = "DepartureDate is required")]
    [DataType(DataType.Date)]
    public DateTime DepartureDate { get; set; }

    [Required,Range(1, 20, ErrorMessage = "Guests must be between 1 and 20")]
    public int Guests { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "MaxPrice must be greater than 0")]
    public decimal? MaxPrice { get; set; }
}