using AutoMapper;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Web_Api.Contracts;

namespace Web_Api.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly ILogger<ReservationsController> _logger;
    private readonly IMapper _mapper;

    public SearchController( ISearchService searchService, ILogger<ReservationsController> logger, IMapper mapper )
    {
        _mapper = mapper;
        _logger = logger;
        _searchService = searchService;
    }

    [HttpGet( "search" )]
    public IActionResult SearchAvailableRooms( [FromQuery] SearchAvailableRoomsContract dto )
    {
        try
        {
            IEnumerable<(Domain.Entities.Property Property, Domain.Entities.RoomType RoomType)> results =
                _searchService.SearchAvailableRooms(
                    dto.City,
                    dto.ArrivalDate,
                    dto.DepartureDate,
                    dto.Guests,
                    dto.MaxPrice );

            List<AvailableRoomResultContract> response = results.Select( x => new AvailableRoomResultContract
            {
                Property = _mapper.Map<PropertyContract>( x.Property ),
                RoomType = _mapper.Map<RoomTypeContract>( x.RoomType ),
                TotalPrice = x.RoomType.DailyPrice * ( dto.DepartureDate - dto.ArrivalDate ).Days
            } ).ToList();

            return Ok( response );
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Search failed" );
            return BadRequest( ex.Message );
        }
    }
}