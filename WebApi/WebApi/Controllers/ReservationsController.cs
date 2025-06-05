using AutoMapper;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Web_Api.Contracts;

namespace Web_Api.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(
        IReservationService reservationService,
        IMapper mapper,
        ILogger<ReservationsController> logger )
    {
        _reservationService = reservationService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult CreateReservation( [FromBody] AddReservationContract dto )
    {
        try
        {
            Reservation reservation = _reservationService.CreateReservation(
                dto.PropertyId,
                dto.RoomTypeId,
                dto.ArrivalDate,
                dto.DepartureDate,
                dto.PersonCount,
                dto.GuestName,
                dto.GuestPhoneNumber );

            ReservationContract? response = _mapper.Map<ReservationContract>( reservation );
            return CreatedAtAction( nameof( GetReservation ), new
            {
                id = reservation.Id
            }, response );
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Reservation creation failed" );
            return BadRequest( ex.Message );
        }
    }

    [HttpGet]
    public IActionResult GetReservations(
        [FromQuery] Guid? propertyId = null,
        [FromQuery] Guid? roomTypeId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? guestName = null )
    {
        IEnumerable<Reservation> reservations = _reservationService.GetReservations(
            propertyId,
            roomTypeId,
            fromDate,
            toDate,
            guestName );

        return Ok( _mapper.Map<List<ReservationContract>>( reservations ) );
    }

    [HttpGet( "{id:guid}" )]
    public IActionResult GetReservation( Guid id )
    {
        Reservation? reservation = _reservationService.GetReservation( id );
        if ( reservation == null )
            return NotFound();

        return Ok( _mapper.Map<ReservationContract>( reservation ) );
    }

    [HttpDelete( "{id:guid}" )]
    public IActionResult CancelReservation( Guid id, [FromQuery] bool softDelete = true )
    {
        try
        {
            _reservationService.CancelReservation( id, softDelete );
            return NoContent();
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Reservation cancellation failed" );
            return BadRequest( ex.Message );
        }
    }
}