using CliniCore.Modules.Appointments.Core.Models;
using CliniCore.Modules.Appointments.Core.OutputPorts;
using CliniCore.Modules.Appointments.Shell.DAL.Entities;

namespace CliniCore.Modules.Appointments.Shell.DAL.Repositories;
internal class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentsDbContext _dbContext;

    public AppointmentRepository(AppointmentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAppointmentAsync(Appointment appointment)
    {
        var appointmentEntity = AppointmentEntity.From(appointment);

        await _dbContext.AddAsync(appointmentEntity);
        await _dbContext.SaveChangesAsync();

        return appointmentEntity.Id;
    }
}
