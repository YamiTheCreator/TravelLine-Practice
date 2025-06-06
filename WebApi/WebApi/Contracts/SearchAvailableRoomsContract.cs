using System.ComponentModel.DataAnnotations;

namespace Web_Api.Contracts;

public class SearchAvailableRoomsContract
{
    [Required] public string City { get; set; }

    [Required] public DateTime ArrivalDate { get; set; }

    [Required] public DateTime DepartureDate { get; set; }

    [Required, Range( 1, 10 )] public int Guests { get; set; }

    public decimal? MaxPrice { get; set; }
}