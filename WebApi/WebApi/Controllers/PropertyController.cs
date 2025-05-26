using AutoMapper;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly IMapper _mapper;

    public PropertiesController(
        IPropertyService propertyService,
        IMapper mapper)
    {
        _propertyService = propertyService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(_mapper.Map<List<PropertyResponseContract>>(properties));
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
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
                return NotFound($"Property with Id {id} not found.");

            return Ok(_mapper.Map<PropertyResponseContract>(property));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddPropertyContract contract)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var property = _mapper.Map<Property>(contract);
            var created = await _propertyService.AddPropertyAsync(property);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                _mapper.Map<PropertyResponseContract>(created));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePropertyContract contract)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != contract.Id)
            return BadRequest("Id mismatch between URL and body.");

        try
        {
            var property = _mapper.Map<Property>(contract);
            await _propertyService.UpdatePropertyAsync(property);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _propertyService.DeletePropertyAsync(id);
            if (!deleted)
                return NotFound($"Property with Id {id} not found.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}