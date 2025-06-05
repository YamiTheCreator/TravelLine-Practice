using System.ComponentModel.DataAnnotations;

namespace Web_Api.Contracts;

public class AddReservationContract
{
    [Required] public Guid PropertyId { get; set; }

    [Required] public Guid RoomTypeId { get; set; }

    [Required] public DateTime ArrivalDate { get; set; }

    [Required] public DateTime DepartureDate { get; set; }

    [Required, Range( 1, 10 )] public int PersonCount { get; set; }

    [Required, MinLength( 2 )] public string GuestName { get; set; }

    [Required, Phone] public string GuestPhoneNumber { get; set; }
}