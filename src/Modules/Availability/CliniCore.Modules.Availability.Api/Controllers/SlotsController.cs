using CliniCore.Modules.Availability.Business.DTO;
using CliniCore.Modules.Availability.Business.Services;
using CliniCore.Modules.Availability.Shared.DTO;
using CliniCore.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CliniCore.Modules.Availability.Api.Controllers;
[Route("api/v1/[controller]")]
[ApiController]
public class SlotsController : ControllerBase
{
    private readonly SlotsService _slotsService;

    public SlotsController(SlotsService slotsService)
    {
        _slotsService = slotsService;
    }

    /// <summary>
    /// Retrieves all slots, including available and reserved slots.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of all slots. Slots include details such as time, cost, and reservation status.
    /// </remarks>
    /// <returns>List of SlotDto objects containing slot details.</returns>
    /// <response code="200">Successfully retrieved the list of slots.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SlotDto>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSlots()
    {
        var result = await _slotsService.GetAllSlotsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves details of a specific slot by its unique ID.
    /// </summary>
    /// <remarks>
    /// Use this endpoint to fetch detailed information about a single slot by providing its ID.
    /// </remarks>
    /// <param name="id">The unique identifier of the slot.</param>
    /// <returns>A SlotDto object containing the slot details.</returns>
    /// <response code="200">Successfully retrieved the slot details.</response>
    /// <response code="404">Slot with the specified ID was not found.</response>
    [HttpGet("{id:guid:required}")]
    [ProducesResponseType(typeof(SlotDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSlotById([FromRoute] Guid id)
    {
        var result = await _slotsService.GetSlotByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Adds a new slot for the doctor.
    /// </summary>
    /// <remarks>
    /// This endpoint allows adding a new time slot with specific details such as time and cost.
    /// </remarks>
    /// <param name="addSlotDto">An object containing the time and cost for the new slot.</param>
    /// <returns>The unique identifier of the newly created slot.</returns>
    /// <response code="201">Slot was successfully created.</response>
    /// <response code="400">The provided slot details were invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSlot([FromBody] AddSlotDto addSlotDto)
    {
        var result = await _slotsService.AddSlotAsync(addSlotDto);
        return CreatedAtAction(nameof(GetSlotById), new { id = result }, null);
    }
}
