using CliniCore.Modules.Appointments.Core.DTO;
using CliniCore.Modules.Appointments.Core.Exceptions;
using CliniCore.Modules.Appointments.Core.InputPorts;
using CliniCore.Modules.Appointments.Core.Models;
using CliniCore.Modules.Appointments.Shell.DTO;
using CliniCore.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CliniCore.Modules.Appointments.Shell.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentsService _appointmentService;

    public AppointmentsController(IAppointmentsService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// Get a list of upcoming appointments.
    /// </summary>
    /// <remarks>
    /// This endpoint retrieves all appointments that are scheduled for the future. The response includes details about each appointment.
    /// </remarks>
    /// <returns>A list of upcoming appointments.</returns>
    /// <response code="200">Returns the list of upcoming appointments.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUpcomingAppointments()
    {
        var result = await _appointmentService.GetUpcomingAppointmentsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Update the status of an appointment.
    /// </summary>
    /// <remarks>
    /// Allows updating the status of an appointment (to "Completed" or "Cancelled").
    /// </remarks>
    /// <param name="id">The ID of the appointment to update.</param>
    /// <param name="request">The new status for the appointment.</param>
    /// <returns>A confirmation message indicating the status update.</returns>
    /// <response code="200">Status updated successfully.</response>
    /// <response code="400">If the status is invalid.</response>
    /// <response code="404">If the appointment ID does not exist.</response>
    [HttpPatch("{id:guid:required}")]
    [ProducesResponseType(typeof(UpdateStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAppointmentStatus(Guid id, [FromBody] UpdateStatusRequest request)
    {
        if (!Enum.TryParse<AppointmentStatus>(request.Status, out var status))
        {
            throw new InvalidStatusException();
        }


        await _appointmentService.UpdateAppointmentStatusAsync(id, new UpdateStatusCommand { Status = status});
        return Ok(new UpdateStatusResponse { Message = "Status has been updated successfully" });
    }
}
