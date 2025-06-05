using AutoMapper;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Web_Api.Contracts;

namespace Web_Api.Controllers;

[Route( "api/[controller]" )]
[ApiController]
public class RoomTypesController : ControllerBase
{
    private readonly IRoomTypeService _roomTypeService;
    private readonly IPropertyService _propertyService;
    private readonly IMapper _mapper;
    private readonly ILogger<RoomTypesController> _logger;

    public RoomTypesController(
        IRoomTypeService roomTypeService,
        IPropertyService propertyService,
        IMapper mapper,
        ILogger<RoomTypesController> logger )
    {
        _roomTypeService = roomTypeService;
        _propertyService = propertyService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet( "properties/{propertyId:guid}/roomtypes" )]
    public ActionResult<IEnumerable<RoomTypeContract>> GetRoomTypesByPropertyId( Guid propertyId )
    {
        Property? property = _propertyService.GetPropertyById( propertyId );
        if ( property == null )
        {
            _logger.LogWarning( "Property with id {PropertyId} not found", propertyId );
            return NotFound( $"Property with id {propertyId} not found" );
        }

        IEnumerable<RoomType> roomTypes = _roomTypeService.GetRoomTypeByPropertyId( propertyId );
        List<RoomTypeContract>? roomTypeContracts = _mapper.Map<List<RoomTypeContract>>( roomTypes );
        return Ok( roomTypeContracts );
    }

    [HttpGet( "{id:guid}" )]
    public ActionResult<RoomTypeContract> GetRoomType( Guid id )
    {
        RoomType? roomType = _roomTypeService.GetRoomTypeById( id );
        if ( roomType == null )
        {
            _logger.LogWarning( "RoomType with id {RoomTypeId} not found", id );
            return NotFound( $"RoomType with id {id} not found" );
        }

        RoomTypeContract? roomTypeDto = _mapper.Map<RoomTypeContract>( roomType );
        return Ok( roomTypeDto );
    }

    [HttpPost( "properties/{propertyId:guid}/roomtypes" )]
    public ActionResult<RoomTypeContract> AddRoomType( Guid propertyId, AddRoomTypeContract addRoomTypeContract )
    {
        Property? property = _propertyService.GetPropertyById( propertyId );
        if ( property == null )
        {
            _logger.LogWarning( "Property with id {PropertyId} not found when adding room type", propertyId );
            return NotFound( $"Property with id {propertyId} not found" );
        }

        RoomType? roomType = _mapper.Map<RoomType>( addRoomTypeContract );
        roomType.PropertyId = propertyId;

        try
        {
            _roomTypeService.AddRoomType( roomType );

            RoomTypeContract? roomTypeContract = _mapper.Map<RoomTypeContract>( roomType );
            return CreatedAtAction( nameof( GetRoomType ), new
            {
                id = roomType.Id
            }, roomTypeContract );
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Error adding room type for property {PropertyId}", propertyId );
            return StatusCode( 500, "Internal server error" );
        }
    }

    [HttpPut( "{id:guid}" )]
    public IActionResult UpdateRoomType( Guid id, UpdateRoomTypeContract updateRoomTypeContract )
    {
        RoomType? existingRoomType = _roomTypeService.GetRoomTypeById( id );
        if ( existingRoomType == null )
        {
            _logger.LogWarning( "RoomType with id {RoomTypeId} not found for update", id );
            return NotFound( $"RoomType with id {id} not found" );
        }

        try
        {
            _mapper.Map( updateRoomTypeContract, existingRoomType );
            _roomTypeService.UpdateRoomType( existingRoomType );

            return NoContent();
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Error updating room type with id {RoomTypeId}", id );
            return StatusCode( 500, "Internal server error" );
        }
    }

    [HttpDelete( "{id:guid}" )]
    public IActionResult DeleteRoomType( Guid id )
    {
        RoomType? roomType = _roomTypeService.GetRoomTypeById( id );
        if ( roomType == null )
        {
            _logger.LogWarning( "RoomType with id {RoomTypeId} not found for deletion", id );
            return NotFound( $"RoomType with id {id} not found" );
        }

        try
        {
            _roomTypeService.DeleteRoomType( id );
            return NoContent();
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Error deleting room type with id {RoomTypeId}", id );
            return StatusCode( 500, "Internal server error" );
        }
    }
}