using AutoFixture;
using CliniCore.Modules.Availability.Business.DTO;
using CliniCore.Modules.Availability.Business.Exceptions;
using CliniCore.Modules.Availability.Business.Mappers;
using CliniCore.Modules.Availability.Business.Services;
using CliniCore.Modules.Availability.Data.Entities;
using CliniCore.Modules.Availability.Data.Repositories;
using CliniCore.Modules.Availability.Shared.DTO;
using CliniCore.Shared.Time;
using FluentAssertions;
using Moq;

namespace CliniCore.Tests.Availability.Business.Services
{
    public class SlotsServiceTests
    {
        private readonly Mock<ISlotRepository> _mockSlotRepository;
        private readonly Mock<ISlotsMapper> _mockSlotsMapper;
        private readonly Mock<IClock> _mockClock;
        private readonly SlotsService _slotsService;
        private readonly IFixture _fixture;

        public SlotsServiceTests()
        {
            _mockSlotRepository = new Mock<ISlotRepository>();
            _mockSlotsMapper = new Mock<ISlotsMapper>();
            _mockClock = new Mock<IClock>();
            _fixture = new Fixture();
            _slotsService = new SlotsService(_mockSlotRepository.Object, _mockSlotsMapper.Object, _mockClock.Object);
        }

        [Fact]
        public async Task GetAllSlotsAsync_ShouldReturnAllSlots()
        {
            // Arrange
            var slotEntities = _fixture.CreateMany<SlotEntity>();
            var slotDtos = _fixture.CreateMany<SlotDto>();

            _mockSlotRepository.Setup(r => r.GetAllSlotsAsync()).ReturnsAsync(slotEntities);
            _mockSlotsMapper.Setup(m => m.MapToDto(slotEntities)).Returns(slotDtos);

            // Act
            var result = await _slotsService.GetAllSlotsAsync();

            // Assert
            result.Should().BeEquivalentTo(slotDtos);
            _mockSlotRepository.Verify(r => r.GetAllSlotsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_ShouldReturnMappedAvailableSlots()
        {
            // Arrange
            var slotEntities = _fixture.CreateMany<SlotEntity>();
            var slotDtos = _fixture.Build<SlotDto>()
                         .With(dto => dto.IsReserved, false)
                         .CreateMany();

            _mockSlotRepository.Setup(r => r.GetAvailableSlotsAsync()).ReturnsAsync(slotEntities);
            _mockSlotsMapper.Setup(m => m.MapToDto(slotEntities)).Returns(slotDtos);

            // Act
            var result = await _slotsService.GetAvailableSlotsAsync();

            // Assert
            result.Should().BeEquivalentTo(slotDtos);
            result.Should().AllSatisfy(x => x.IsReserved.Should().BeFalse());
            _mockSlotRepository.Verify(r => r.GetAvailableSlotsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetSlotByIdAsync_ShouldReturnMappedSlot_WhenSlotExists()
        {
            // Arrange
            var slotId = Guid.NewGuid();
            var slotEntity = _fixture.Create<SlotEntity>();
            var slotDto = _fixture.Create<SlotDto>();

            _mockSlotRepository.Setup(r => r.GetSlotByIdAsync(slotId)).ReturnsAsync(slotEntity);
            _mockSlotsMapper.Setup(m => m.MapToDto(slotEntity)).Returns(slotDto);

            // Act
            var result = await _slotsService.GetSlotByIdAsync(slotId);

            // Assert
            result.Should().BeEquivalentTo(slotDto);
            _mockSlotRepository.Verify(r => r.GetSlotByIdAsync(slotId), Times.Once);
        }

        [Fact]
        public async Task GetSlotByIdAsync_ShouldThrowSlotNotFoundException_WhenSlotDoesNotExist()
        {
            // Arrange
            var slotId = Guid.NewGuid();
            _mockSlotRepository.Setup(r => r.GetSlotByIdAsync(slotId)).ReturnsAsync((SlotEntity)null);

            // Act & Assert
            await _slotsService.Invoking(slotService => slotService.GetSlotByIdAsync(slotId))
                .Should().ThrowAsync<SlotNotFoundException>();
            _mockSlotRepository.Verify(r => r.GetSlotByIdAsync(slotId), Times.Once);
        }

        [Fact]
        public async Task AddSlotAsync_WithValidSlot_ShouldReturnSlotId()
        {
            // Arrange
            var addSlotDto = _fixture.Build<AddSlotDto>()
                         .With(dto => dto.Time, DateTime.UtcNow.AddDays(1))
                         .With(dto => dto.Cost, 100)
                         .Create();
            var slotEntity = _fixture.Build<SlotEntity>()
                         .With(dto => dto.Time, DateTime.UtcNow.AddDays(1))
                         .With(dto => dto.Cost, 100)
                         .Create();
            var slotId = Guid.NewGuid();

            _mockClock.Setup(c => c.CurrentDate()).Returns(DateTime.UtcNow);
            _mockSlotsMapper.Setup(m => m.MapFrom(addSlotDto)).Returns(slotEntity);
            _mockSlotRepository.Setup(r => r.AddSlotAsync(slotEntity)).ReturnsAsync(slotId);

            // Act
            var result = await _slotsService.AddSlotAsync(addSlotDto);

            // Assert
            result.Should().Be(slotId);
            _mockSlotRepository.Verify(r => r.AddSlotAsync(slotEntity), Times.Once);
        }

        [Fact]
        public async Task AddSlotAsync_ShouldThrowInvalidSlotTimeException_WithPastSlotTime()
        {
            // Arrange
            var addSlotDto = _fixture.Build<AddSlotDto>()
                         .With(dto => dto.Time, DateTime.UtcNow.AddDays(-1))
                         .With(dto => dto.Cost, 100)
                         .Create();

            _mockClock.Setup(c => c.CurrentDate()).Returns(DateTime.UtcNow);

            // Act & Assert
            await _slotsService.Invoking(slotService => slotService.AddSlotAsync(addSlotDto))
                .Should().ThrowAsync<InvalidSlotTimeException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task AddSlotAsync_ShouldThrowInvalidSlotCostException_WhenCostIsInvalid(decimal invalidCost)
        {
            // Arrange
            var addSlotDto = _fixture.Build<AddSlotDto>()
                         .With(dto => dto.Time, DateTime.UtcNow.AddDays(1))
                         .With(dto => dto.Cost, invalidCost)
                         .Create();

            _mockClock.Setup(c => c.CurrentDate()).Returns(DateTime.UtcNow);

            // Act & Assert
            await _slotsService.Invoking(slotService => slotService.AddSlotAsync(addSlotDto))
                .Should().ThrowAsync<InvalidSlotCostException>();
        }
    }
}
