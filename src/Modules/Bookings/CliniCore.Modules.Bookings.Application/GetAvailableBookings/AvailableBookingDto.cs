namespace CliniCore.Modules.Bookings.Application.GetAvailableBookings;
public class AvailableBookingDto
{
    public Guid Id { get; set; }
    public DateTime Time { get; set; }
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; }
    public bool IsReserved { get; set; }
    public decimal Cost { get; set; }
}
