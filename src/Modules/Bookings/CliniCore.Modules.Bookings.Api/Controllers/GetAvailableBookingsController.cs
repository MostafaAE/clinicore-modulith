using CliniCore.Modules.Bookings.Application.GetAvailableBookings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CliniCore.Modules.Bookings.Api.Controllers;

[Route("api/v1/Bookings")]
[ApiController]
public class GetAvailableBookingsController : ControllerBase
{
    private readonly GetAvailableBookingsHandler _getAvailableBookingHandler;

    public GetAvailableBookingsController(GetAvailableBookingsHandler getAvailableBookingHandler)
    {
        _getAvailableBookingHandler = getAvailableBookingHandler;
    }

    /// <summary>
    /// Get all available bookings.
    /// </summary>
    /// <remarks>
    /// Retrieves a list of all available slots that can be booked.
    /// </remarks>
    /// <returns>A list of available bookings.</returns>
    /// <response code="200">Returns the list of available bookings.</response>
    [HttpGet("Available")]
    [ProducesResponseType(typeof(IEnumerable<AvailableBookingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableBookings()
    {
        var availableBookings = await _getAvailableBookingHandler.Handle();

        return Ok(availableBookings);
    }
}
