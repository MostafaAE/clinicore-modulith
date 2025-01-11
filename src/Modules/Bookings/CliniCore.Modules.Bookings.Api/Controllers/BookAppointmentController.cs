using CliniCore.Modules.Bookings.Application.BookAppointment;
using Microsoft.AspNetCore.Mvc;

namespace CliniCore.Modules.Bookings.Api.Controllers;

[Route("api/v1/Bookings")]
[ApiController]
public class BookAppointmentController : ControllerBase
{
    private readonly AddBookingHandler _addBookingHandler;

    public BookAppointmentController(AddBookingHandler addBookingHandler)
    {
        _addBookingHandler = addBookingHandler;
    }

    [HttpPost]
    public async Task<IActionResult> AddBooking(AddBooking command)
    {
        var result = await _addBookingHandler.Handle(command);
        return Ok(result);
    }
}
