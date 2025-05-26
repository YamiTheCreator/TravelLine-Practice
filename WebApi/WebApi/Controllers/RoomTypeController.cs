using AutoMapper;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;

namespace WebApi.Controllers;

[ApiController]
public class RoomTypeController : ControllerBase
{
    private readonly IRoomTypeService _roomTypeService;
    private readonly IMapper _mapper;

    public RoomTypeController(
        IRoomTypeService roomTypeService,
        IMapper mapper)
    {
        _roomTypeService = roomTypeService;
        _mapper = mapper;
    }

    [HttpGet("api/properties/{propertyId:guid}/roomtypes")]
    public async Task<IActionResult> GetByPropertyId(Guid propertyId)
    {
        var roomTypes = await _roomTypeService.GetRoomTypesByPropertyIdAsync(propertyId);
        return Ok(_mapper.Map<List<RoomTypeResponseContract>>(roomTypes));
    }

    [HttpGet("api/roomtypes/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var roomType = await _roomTypeService.GetRoomTypeByIdAsync(id);
        if (roomType == null)
            return NotFound();

        return Ok(_mapper.Map<RoomTypeResponseContract>(roomType));
    }

    [HttpPost("api/properties/{propertyId:guid}/roomtypes")]
    public async Task<IActionResult> Create(Guid propertyId, [FromBody] AddRoomTypeContract contract)
    {
        if (propertyId != contract.PropertyId)
            return BadRequest("PropertyId in URL and body do not match.");

        var roomType = _mapper.Map<RoomType>(contract);
        var created = await _roomTypeService.CreateRoomTypeForPropertyAsync(propertyId, roomType);
        var response = _mapper.Map<RoomTypeResponseContract>(created);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("api/roomtypes/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomTypeContract contract)
    {
        if (id != contract.Id)
            return BadRequest("Id mismatch");

        var roomType = _mapper.Map<RoomType>(contract);

        try
        {
            var updated = await _roomTypeService.UpdateRoomTypeAsync(roomType);
            return Ok(_mapper.Map<RoomTypeResponseContract>(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("api/roomtypes/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _roomTypeService.DeleteRoomTypeAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}