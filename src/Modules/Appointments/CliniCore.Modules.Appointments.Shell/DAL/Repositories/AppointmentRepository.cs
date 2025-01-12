using CliniCore.Modules.Appointments.Core.Models;
using CliniCore.Modules.Appointments.Core.OutputPorts;
using CliniCore.Modules.Appointments.Shell.DAL.Entities;
using CliniCore.Shared.Time;
using Microsoft.EntityFrameworkCore;

namespace CliniCore.Modules.Appointments.Shell.DAL.Repositories;
internal class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentsDbContext _dbContext;
    private readonly IClock _clock;

    public AppointmentRepository(AppointmentsDbContext dbContext, IClock clock)
    {
        _dbContext = dbContext;
        _clock = clock;
    }

    public async Task<Guid> AddAppointmentAsync(Appointment appointment)
    {
        var appointmentEntity = AppointmentEntity.From(appointment);

        await _dbContext.AddAsync(appointmentEntity);
        await _dbContext.SaveChangesAsync();

        return appointmentEntity.Id;
    }

    public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync()
    {
        var appointmentsEntities = await _dbContext.Appointments
                                        .Where(appointment => appointment.Time > _clock.CurrentDate())
                                        .ToListAsync();

        var appointmentsDomain = appointmentsEntities.Select(entity => entity.ToDomain());
        return appointmentsDomain;
    }
}
