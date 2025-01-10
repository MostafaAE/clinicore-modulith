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

namespace CliniCore.Modules.Availability.Tests.Business.Services
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

        
    }
}
