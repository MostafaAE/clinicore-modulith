using CliniCore.Modules.Availability.Business.DTO;
using CliniCore.Modules.Availability.Business.Exceptions;
using CliniCore.Modules.Availability.Business.Mappers;
using CliniCore.Modules.Availability.Data.Repositories;
using CliniCore.Modules.Availability.Shared.DTO;
using CliniCore.Shared.Time;

namespace CliniCore.Modules.Availability.Business.Services;
public class SlotsService
{
    private readonly ISlotRepository _slotRepository;
    private readonly ISlotsMapper _slotsMapper;
    private readonly IClock _clock;

    public SlotsService(ISlotRepository slotRepository, ISlotsMapper slotsMapper, IClock clock)
    {
        _slotRepository = slotRepository;
        _slotsMapper = slotsMapper;
        _clock = clock;
    }

    public async Task<IEnumerable<SlotDto>> GetAllSlotsAsync()
    {
        var slotEntities = await _slotRepository.GetAllSlotsAsync();

        var slotDtos = _slotsMapper.MapToDto(slotEntities);

        return slotDtos;
    }
    
    public async Task<IEnumerable<SlotDto>> GetAvailableSlotsAsync()
    {
        var slotEntities = await _slotRepository.GetAvailableSlotsAsync();

        var slotDtos = _slotsMapper.MapToDto(slotEntities);

        return slotDtos;
    }

    public async Task<SlotDto> GetSlotByIdAsync(Guid id)
    {
        var slotEntity = await _slotRepository.GetSlotByIdAsync(id);

        if (slotEntity is null)
            throw new SlotNotFoundException();

        var slotDto = _slotsMapper.MapToDto(slotEntity);

        return slotDto;
    }

    public async Task<Guid> AddSlotAsync(AddSlotDto addSlotDto)
    {
        if(addSlotDto.Time < _clock.CurrentDate())
        {
            throw new InvalidSlotTimeException();
        }

        if(addSlotDto.Cost <= 0)
        {
            throw new InvalidSlotCostException();
        }

        var slotEntity = _slotsMapper.MapFrom(addSlotDto);

        return await _slotRepository.AddSlotAsync(slotEntity);
    }

    public async Task ReserveSlotAsync(Guid id)
    {
        var slotEntity = await _slotRepository.GetSlotByIdAsync(id);

        if (slotEntity is null)
            throw new SlotNotFoundException();

        if(slotEntity.IsReserved == true)
            throw new SlotAlreadyReservedException();

        slotEntity.IsReserved = true;
        await _slotRepository.UpdateSlotAsync(slotEntity);
    }
}
