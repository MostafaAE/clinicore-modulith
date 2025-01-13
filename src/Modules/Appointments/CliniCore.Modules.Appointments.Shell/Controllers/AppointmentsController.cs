using CliniCore.Modules.Appointments.Core.DTO;
using CliniCore.Modules.Appointments.Core.Exceptions;
using CliniCore.Modules.Appointments.Core.InputPorts;
using CliniCore.Modules.Appointments.Core.Models;
using CliniCore.Modules.Appointments.Shell.DTO;
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

    [HttpGet]
    public async Task<IActionResult> GetUpcomingAppointments()
    {
        var result = await _appointmentService.GetUpcomingAppointmentsAsync();
        return Ok(result);
    }

    [HttpPatch("{id:guid:required}")]
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
