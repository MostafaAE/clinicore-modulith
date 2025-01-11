using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using CliniCore.Modules.Availability.Business.DTO;
using CliniCore.Modules.Availability.Data.Entities;
using CliniCore.Modules.Availability.Shared.DTO;
using CliniCore.Tests.Shared.Utils;
using CliniCore.Tests.Shared;
using FluentAssertions;
using CliniCore.Modules.Availability.Data;

namespace CliniCore.Tests.Availability.Api.Controllers;

public class SlotsControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly CustomWebApplicationFactory _factory;
    private readonly IFixture _fixture;

    public SlotsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
        _fixture = new Fixture();
    }

    public async Task InitializeAsync()
    {
        await TestUtils.ClearDatabaseAsync(_factory);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetAllSlots_ShouldReturnOkWithSlots()
    {
        // Arrange
        var slots = _fixture.CreateMany<SlotEntity>().ToList();
        await TestUtils.AddToDatabaseAsync<AvailabilityDbContext, SlotEntity>(_factory, slots);

        // Act
        var response = await _httpClient.GetAsync("/api/v1/Slots");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<SlotDto>>();
        result.Should().NotBeNull().And.HaveCount(slots.Count);
    }


    [Fact]
    public async Task GetSlotById_ShouldReturnOkWithSlot_WhenSlotExists()
    {
        // Arrange
        var slot = _fixture.Create<SlotEntity>();
        await TestUtils.AddToDatabaseAsync<AvailabilityDbContext, SlotEntity>(_factory, slot);

        // Act
        var response = await _httpClient.GetAsync($"/api/v1/Slots/{slot.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<SlotDto>();
        result.Should().NotBeNull();
        result.Id.Should().Be(slot.Id);
    }

    [Fact]
    public async Task GetSlotById_ShouldReturnNotFound_WhenSlotDoesNotExist()
    {
        // Arrange
        var nonExistentSlotId = Guid.NewGuid();

        // Act
        var response = await _httpClient.GetAsync($"/api/v1/Slots/{nonExistentSlotId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddSlot_ShouldReturnCreated_WhenValidSlotIsProvided()
    {
        // Arrange
        var addSlotDto = _fixture.Build<AddSlotDto>()
                                 .With(dto => dto.Time, DateTime.UtcNow.AddDays(1))
                                 .With(dto => dto.Cost, 100)
                                 .Create();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/v1/Slots", addSlotDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task AddSlot_ShouldReturnBadRequest_WhenSlotTimeIsInvalid()
    {
        // Arrange
        var addSlotDto = _fixture.Build<AddSlotDto>()
                                 .With(dto => dto.Time, DateTime.UtcNow.AddDays(-1)) // Invalid time
                                 .With(dto => dto.Cost, 100)
                                 .Create();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/v1/Slots", addSlotDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task AddSlot_ShouldReturnBadRequest_WhenSlotCostIsInvalid(decimal cost)
    {
        // Arrange
        var addSlotDto = _fixture.Build<AddSlotDto>()
                                 .With(dto => dto.Time, DateTime.UtcNow.AddDays(1))
                                 .With(dto => dto.Cost, cost)
                                 .Create();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/v1/Slots", addSlotDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
