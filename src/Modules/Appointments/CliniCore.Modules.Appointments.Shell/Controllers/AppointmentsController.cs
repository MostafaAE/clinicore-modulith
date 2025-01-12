using CliniCore.Modules.Appointments.Core.InputPorts;
using Microsoft.AspNetCore.Mvc;

namespace CliniCore.Modules.Appointments.Shell.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentsService appointmentService;

    public AppointmentsController(IAppointmentsService appointmentService)
    {
        this.appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUpcomingAppointments()
    {
        var result = await appointmentService.GetUpcomingAppointments();
        return Ok(result);
    }
}
