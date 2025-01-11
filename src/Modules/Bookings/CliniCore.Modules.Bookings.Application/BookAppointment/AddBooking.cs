namespace CliniCore.Modules.Bookings.Application.BookAppointment;
public class AddBooking
{
    public Guid SlotId { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; }
}
