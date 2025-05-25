using AutoMapper;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;

    public ReservationsController(
        IReservationService reservationService,
        IMapper mapper)
    {
        _reservationService = reservationService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? propertyId,
        [FromQuery] Guid? roomTypeId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? guestName)
    {
        try
        {
            var reservations = await _reservationService.GetReservationsAsync(
                propertyId, roomTypeId, startDate, endDate, guestName);

            return Ok(_mapper.Map<List<ReservationResponse>>(reservations));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
                return NotFound($"Reservation with Id {id} not found.");

            return Ok(_mapper.Map<ReservationResponse>(reservation));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddReservationRequestContract request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Дополнительная валидация дат
        if (request.ArrivalDate >= request.DepartureDate)
            return BadRequest("Departure date must be after arrival date.");

        if (request.ArrivalDate < DateTime.Today)
            return BadRequest("Arrival date cannot be in the past.");

        try
        {
            var reservation = _mapper.Map<Reservation>(request);
            var created = await _reservationService.CreateReservationAsync(reservation);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                _mapper.Map<ReservationResponse>(created));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        try
        {
            var result = await _reservationService.CancelReservationAsync(id);
            if (!result)
                return NotFound($"Reservation with Id {id} not found or already cancelled.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}