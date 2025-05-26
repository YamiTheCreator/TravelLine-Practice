using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Domain.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;

    public SearchController(
        IPropertyRepository propertyRepository,
        IReservationService reservationService,
        IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _reservationService = reservationService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] [Required] string city,
        [FromQuery] [Required] DateTime arrivalDate,
        [FromQuery] [Required] DateTime departureDate,
        [FromQuery] [Required, Range(1, 20)] int guests,
        [FromQuery] decimal? maxPrice)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (arrivalDate >= departureDate)
            return BadRequest("Departure date must be after arrival date.");

        if (arrivalDate < DateTime.Today)
            return BadRequest("Arrival date cannot be in the past.");

        try
        {
            var properties = await _propertyRepository.GetAllAsync();
            var results = new List<SearchResultContract>();

            foreach (var property in properties.Where(p =>
                         p.City.Equals(city, StringComparison.OrdinalIgnoreCase)))
            {
                foreach (var roomType in property.RoomTypes)
                {
                    if (roomType.MinPersonCount > guests || roomType.MaxPersonCount < guests)
                        continue;

                    if (maxPrice.HasValue && roomType.DailyPrice > maxPrice)
                        continue;

                    var isAvailable = await _reservationService.IsRoomTypeAvailableAsync(
                        roomType.Id, arrivalDate, departureDate);

                    if (!isAvailable)
                        continue;

                    var nights = (departureDate - arrivalDate).Days;
                    results.Add(new SearchResultContract
                    {
                        Property = _mapper.Map<PropertySearchResult>(property),
                        RoomType = _mapper.Map<RoomTypeSearchResult>(roomType),
                        TotalPrice = roomType.DailyPrice * nights,
                        Currency = roomType.Currency,
                        Nights = nights
                    });
                }
            }

            results = results.OrderBy(r => r.TotalPrice).ToList();

            return Ok(results);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}