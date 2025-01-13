using CliniCore.Modules.Appointments.Core.Exceptions;

namespace CliniCore.Modules.Appointments.Core.Models;
public class Appointment
{
    public Guid Id { get; private set; }
    public Guid BookingId { get; private set; }
    public Guid SlotId { get; private set; }
    public DateTime Time { get; private set; }
    public Guid DoctorId { get; private set; }
    public string DoctorName { get; private set; }
    public Guid PatientId { get; private set; }
    public string PatientName { get; private set; }
    public DateTime ReservedAt { get; private set; }
    public decimal Cost { get; private set; }
    public AppointmentStatus Status { get; private set; }

    private Appointment(Guid id, Guid bookingId, Guid slotId, DateTime time, Guid doctorId, string doctorName,
        Guid patientId, string patientName, DateTime reservedAt, decimal cost, AppointmentStatus status) 
    { 
        Id = id;

        if (bookingId == Guid.Empty)
        {
            throw new InvalidBookingIdException();
        }
        BookingId = bookingId;

        if (slotId == Guid.Empty)
        {
            throw new InvalidSlotIdException();
        }
        SlotId = slotId;

        Time = time;

        if (doctorId == Guid.Empty)
        {
            throw new InvalidDoctorIdException();
        }
        DoctorId = doctorId;
        DoctorName = doctorName;

        if(patientId == Guid.Empty)
        {
            throw new InvalidPatientIdException();
        }
        PatientId = patientId;
        PatientName = patientName;

        ReservedAt = reservedAt;
        Cost = cost;
        Status = status;
    }

    public static Appointment NewAppointment(Guid bookingId, Guid slotId, DateTime time, Guid doctorId, string doctorName,
        Guid patientId, string patientName, DateTime reservedAt, decimal cost)
    {
        return new Appointment(Guid.NewGuid(), bookingId, slotId, time, doctorId, doctorName,
            patientId, patientName, reservedAt, cost, AppointmentStatus.Booked);
    }

    public static Appointment Create(Guid id, Guid bookingId, Guid slotId, DateTime time, Guid doctorId, string doctorName,
        Guid patientId, string patientName, DateTime reservedAt, decimal cost, AppointmentStatus status)
    {
        return new Appointment(id, bookingId, slotId, time, doctorId, doctorName,
            patientId, patientName, reservedAt, cost, status);
    }

    public void Complete()
    {
        Status = AppointmentStatus.Completed;
    }

    public void Cancel()
    {
        Status = AppointmentStatus.Canceled;
    }
}
