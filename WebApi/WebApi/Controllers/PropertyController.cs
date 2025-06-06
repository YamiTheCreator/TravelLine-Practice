using AutoMapper;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Web_Api.Contracts;

namespace Web_Api.Controllers;

[ApiController]
[Route( "api/[controller]" )]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly IMapper _mapper;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(
        IPropertyService propertyService,
        IMapper mapper,
        ILogger<PropertiesController> logger )
    {
        _propertyService = propertyService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PropertyContract>> GetAllProperties()
    {
        try
        {
            IEnumerable<Property> properties = _propertyService.GetAllProperties();
            IEnumerable<PropertyContract>? propertyContracts = _mapper.Map<IEnumerable<PropertyContract>>( properties );
            return Ok( propertyContracts );
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Error occurred while getting all properties" );
            return StatusCode( 500, "Internal server error" );
        }
    }

    [HttpGet( "{id:guid}" )]
    public ActionResult<PropertyContract> GetPropertyById( Guid id )
    {
        try
        {
            Property? property = _propertyService.GetPropertyById( id );

            if ( property == null )
            {
                _logger.LogWarning( "Property with id {Id} not found", id );
                return NotFound();
            }

            PropertyContract? propertyContract = _mapper.Map<PropertyContract>( property );
            return Ok( propertyContract );
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Error occurred while getting property with id: {Id}", id );
            return StatusCode( 500, "Internal server error" );
        }
    }

    [HttpPost]
    public IActionResult AddProperty( [FromBody] AddPropertyContract addPropertyContract )
    {
        try
        {
            if ( !ModelState.IsValid )
            {
                _logger.LogWarning( "Invalid model state for AddProperty" );
                return BadRequest( ModelState );
            }

            Property? property = _mapper.Map<Property>( addPropertyContract );
            _propertyService.AddProperty( property );

            PropertyContract? propertyContract = _mapper.Map<PropertyContract>( property );
            _logger.LogInformation( "Property added successfully with id: {Id}", property.Id );

            return CreatedAtAction( nameof( GetPropertyById ), new
            {
                id = propertyContract.Id
            }, propertyContract );
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Error occurred while adding property" );
            return StatusCode( 500, "Internal server error" );
        }
    }

    [HttpPut( "{id:guid}" )]
    public IActionResult UpdateProperty( Guid id, [FromBody] UpdatePropertyContract updatePropertyContract )
    {
        try
        {
            if ( !ModelState.IsValid )
            {
                _logger.LogWarning( "Invalid model state for UpdateProperty with id: {Id}", id );
                return BadRequest( ModelState );
            }

            Property? existingProperty = _propertyService.GetPropertyById( id );
            if ( existingProperty == null )
            {
                _logger.LogWarning( "Property with id {Id} not found for update", id );
                return NotFound();
            }

            _mapper.Map( updatePropertyContract, existingProperty );
            _propertyService.UpdateProperty( existingProperty );

            _logger.LogInformation( "Property with id {Id} updated successfully", id );
            return NoContent();
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Error occurred while updating property with id: {Id}", id );
            return StatusCode( 500, "Internal server error" );
        }
    }

    [HttpDelete( "{id:guid}" )]
    public IActionResult DeleteProperty( Guid id )
    {
        try
        {
            Property? property = _propertyService.GetPropertyById( id );
            if ( property == null )
            {
                _logger.LogWarning( "Property with id {Id} not found for deletion", id );
                return NotFound();
            }

            _propertyService.DeleteProperty( id );
            _logger.LogInformation( "Property with id {Id} deleted successfully", id );

            return NoContent();
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, "Error occurred while deleting property with id: {Id}", id );
            return StatusCode( 500, "Internal server error" );
        }
    }
}