using CliniCore.Modules.Availability.Business.DTO;
using CliniCore.Modules.Availability.Data.Entities;
using CliniCore.Modules.Availability.Shared.DTO;

namespace CliniCore.Modules.Availability.Business.Mappers;
public interface ISlotsMapper
{
    SlotDto MapToDto(SlotEntity slot);
    IEnumerable<SlotDto> MapToDto(IEnumerable<SlotEntity> slots);
    SlotEntity MapFrom(AddSlotDto addSlotDto);
}
