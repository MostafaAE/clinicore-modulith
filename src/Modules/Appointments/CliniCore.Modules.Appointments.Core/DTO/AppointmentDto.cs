using CliniCore.Modules.Appointments.Core.Models;

namespace CliniCore.Modules.Appointments.Core.DTO;

public class AppointmentDto
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
    public AppointmentStatus Status { get; set; }
}