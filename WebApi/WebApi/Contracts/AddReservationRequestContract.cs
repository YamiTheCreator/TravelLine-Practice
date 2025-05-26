using System.ComponentModel.DataAnnotations;

namespace WebApi.Contracts;

public class AddReservationRequestContract
{
    [Required(ErrorMessage = "PropertyId is required")]
    public Guid PropertyId { get; set; }

    [Required(ErrorMessage = "RoomTypeId is required")]
    public Guid RoomTypeId { get; set; }

    [Required(ErrorMessage = "ArrivalDate is required")]
    [DataType(DataType.Date)]
    public DateTime ArrivalDate { get; set; }

    [Required(ErrorMessage = "DepartureDate is required")]
    [DataType(DataType.Date)]
    public DateTime DepartureDate { get; set; }

    [Required(ErrorMessage = "ArrivalTime is required")]
    public TimeSpan ArrivalTime { get; set; }

    [Required(ErrorMessage = "DepartureTime is required")]
    public TimeSpan DepartureTime { get; set; }

    [Required(ErrorMessage = "GuestName is required")]
    [StringLength(200, ErrorMessage = "GuestName cannot exceed 200 characters")]
    public string GuestName { get; set; } = null!;

    [Required(ErrorMessage = "GuestPhoneNumber is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string GuestPhoneNumber { get; set; } = null!;
}