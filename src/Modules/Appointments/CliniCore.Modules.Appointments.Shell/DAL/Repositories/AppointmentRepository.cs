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
                                        .Where(appointment => appointment.Time > _clock.CurrentDate() &&
                                               appointment.Status == AppointmentStatus.Booked)
                                        .ToListAsync();

        var appointmentsDomain = appointmentsEntities.Select(entity => entity.ToDomain());
        return appointmentsDomain;
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(Guid id)
    {
        var appointmentEntity = await _dbContext.Appointments
            .AsNoTracking()
            .FirstOrDefaultAsync(appointment => appointment.Id == id);

        var appointmentDomain = appointmentEntity?.ToDomain();

        return appointmentDomain;
    }

    public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
    {
        var appointmentEntity = AppointmentEntity.From(appointment);

        _dbContext.Update(appointmentEntity);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
