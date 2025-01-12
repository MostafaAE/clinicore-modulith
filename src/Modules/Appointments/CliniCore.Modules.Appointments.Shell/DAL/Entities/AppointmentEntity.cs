using CliniCore.Modules.Appointments.Core.Models;

namespace CliniCore.Modules.Appointments.Shell.DAL.Entities;
public class AppointmentEntity
{
    public Guid Id { get; set; }
    public Guid BookingId { get; set; }
    public Guid SlotId { get; set; }
    public DateTime Time { get; set; }
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; }
    public DateTime ReservedAt { get; set; }
    public decimal Cost { get; set; }

    public static AppointmentEntity From(Appointment appointment)
    {
        return new AppointmentEntity()
        {
            Id = appointment.Id,
            BookingId = appointment.BookingId,
            SlotId = appointment.SlotId,
            Time = appointment.Time,
            DoctorId = appointment.DoctorId,
            DoctorName = appointment.DoctorName,
            PatientId = appointment.PatientId,
            PatientName = appointment.PatientName,
            ReservedAt = appointment.ReservedAt,
            Cost = appointment.Cost,
        };
    }

    public Appointment ToDomain()
    {
        return Appointment.Create(Id, BookingId, SlotId, Time, DoctorId, DoctorName,
            PatientId, PatientName, ReservedAt, Cost);
    }
}
