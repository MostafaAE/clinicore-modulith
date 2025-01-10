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

namespace CliniCore.Modules.Availability.Tests.Integration.Controllers;

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

}
