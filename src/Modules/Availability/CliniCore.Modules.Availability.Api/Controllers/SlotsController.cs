using CliniCore.Modules.Availability.Business.DTO;
using CliniCore.Modules.Availability.Business.Services;
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSlots()
    {
        var result = await _slotsService.GetAllSlotsAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid:required}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSlotById([FromRoute] Guid id)
    {
        var result = await _slotsService.GetSlotByIdAsync(id);
        return Ok(result);
    }
}
