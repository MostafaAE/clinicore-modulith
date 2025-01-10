namespace CliniCore.Modules.Availability.Data.Entities;
public class SlotEntity
{
    public Guid Id { get; set; }
    public DateTime Time { get; set; }
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; }
    public bool IsReserved { get; set; }
    public decimal Cost { get; set; }
}
