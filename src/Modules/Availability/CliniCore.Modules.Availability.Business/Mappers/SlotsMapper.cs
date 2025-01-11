using CliniCore.Modules.Availability.Business.DTO;
using CliniCore.Modules.Availability.Data.Entities;
using CliniCore.Modules.Availability.Shared.DTO;

namespace CliniCore.Modules.Availability.Business.Mappers;
internal class SlotsMapper : ISlotsMapper
{
    public IEnumerable<SlotDto> MapToDto(IEnumerable<SlotEntity> slots)
    {
        var slotDtos = slots.Select(slot => MapToDto(slot));
        return slotDtos;
    }

    public SlotDto MapToDto(SlotEntity slot)
    {
        var slotDto = new SlotDto()
        {
            Id = slot.Id,
            DoctorId = slot.DoctorId,
            DoctorName = slot.DoctorName,
            IsReserved = slot.IsReserved,
            Time = slot.Time,
            Cost = slot.Cost,
        };

        return slotDto;
    }

    public SlotEntity MapFrom(AddSlotDto addSlotDto)
    {
        var slot = new SlotEntity()
        {
            DoctorId = Guid.NewGuid(),
            DoctorName = "Mostafa",
            Cost = addSlotDto.Cost,
            IsReserved = false,
            Time = addSlotDto.Time,
        };

        return slot;
    }
}
