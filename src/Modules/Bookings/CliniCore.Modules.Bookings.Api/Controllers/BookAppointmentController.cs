using CliniCore.Modules.Bookings.Application.BookAppointment;
using CliniCore.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
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

    /// <summary>
    /// Add a new booking for a specific slot.
    /// </summary>
    /// <remarks>
    /// Allows a patient to book a specific slot. The patient must provide their details and the slot ID.
    /// </remarks>
    /// <param name="command">The booking details, including SlotId, PatientId, and PatientName.</param>
    /// <returns>A confirmation of the booking, including the Booking ID.</returns>
    /// <response code="200">Returns the newly created booking ID.</response>
    /// <response code="400">If the input is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddBooking(AddBooking command)
    {
        var result = await _addBookingHandler.Handle(command);
        return Ok(result);
    }
}
