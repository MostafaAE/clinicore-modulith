using CliniCore.Modules.Bookings.Application.GetAvailableBookings;
using Microsoft.AspNetCore.Mvc;

namespace CliniCore.Modules.Bookings.Api.Controllers;

[Route("api/v1/AvailableBookings")]
[ApiController]
public class GetAvailableBookingsController : ControllerBase
{
    private readonly GetAvailableBookingsHandler _getAvailableBookingHandler;

    public GetAvailableBookingsController(GetAvailableBookingsHandler getAvailableBookingHandler)
    {
        _getAvailableBookingHandler = getAvailableBookingHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableBookings()
    {
        var availableBookings = await _getAvailableBookingHandler.Handle();

        return Ok(availableBookings);
    }
}
