using AutoFixture;
using CliniCore.Modules.Availability.Business.Exceptions;
using CliniCore.Modules.Availability.Shared;
using CliniCore.Modules.Availability.Shared.DTO;
using CliniCore.Modules.Bookings.Application.BookAppointment;
using CliniCore.Modules.Bookings.Infrastructure.DAL;
using CliniCore.Modules.Bookings.Infrastructure.DAL.Entities;
using CliniCore.Tests.Shared;
using CliniCore.Tests.Shared.Utils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace CliniCore.Tests.Bookings.Api;
public class BookAppointmentControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly Fixture _fixture;
    private readonly HttpClient _httpClient;
    private readonly Mock<IAvailabilityModuleApi> _availabilityModuleApiMock;

    public BookAppointmentControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _availabilityModuleApiMock = new Mock<IAvailabilityModuleApi>();
        _httpClient = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(_ => _availabilityModuleApiMock.Object);
            });
        }).CreateClient();
    }

    public async Task InitializeAsync()
    {
        await TestUtils.ClearDatabaseAsync(_factory);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task AddBooking_ShouldReturnOk_WhenBookingIsSuccessful()
    {
        // Arrange
        var command = _fixture.Create<AddBooking>();

        var slot = _fixture.Build<SlotDto>()
            .With(slot => slot.IsReserved, false).Create();

        _availabilityModuleApiMock
            .Setup(api => api.GetSlotByIdAsync(command.SlotId))
            .ReturnsAsync(slot);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/v1/Bookings", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseData = await response.Content.ReadFromJsonAsync<BookingResponse>();

        var booking = await TestUtils.GetFromDatabaseAsync<BookingDbContext, BookingEntity>(_factory, responseData.Id);

        _availabilityModuleApiMock.Verify(api => api.GetSlotByIdAsync(command.SlotId), Times.Once);
        booking.Should().NotBeNull();
    }

    [Fact]
    public async Task AddBooking_ShouldReturnBadRequest_WhenSlotIsAlreadyBooked()
    {
        // Arrange
        var command = _fixture.Create<AddBooking>();
        var slot = _fixture.Build<SlotDto>()
            .With(slot => slot.IsReserved, true).Create();

        _availabilityModuleApiMock
            .Setup(api => api.GetSlotByIdAsync(command.SlotId))
            .ReturnsAsync(slot);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/v1/Bookings", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        _availabilityModuleApiMock.Verify(api => api.GetSlotByIdAsync(command.SlotId), Times.Once);
    }

    [Fact]
    public async Task AddBooking_ShouldReturnNotFound_WhenSlotDoesNotExist()
    {
        // Arrange
        var command = _fixture.Create<AddBooking>();

        _availabilityModuleApiMock
            .Setup(api => api.GetSlotByIdAsync(command.SlotId))
            .ThrowsAsync(new SlotNotFoundException());

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/v1/Bookings", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        _availabilityModuleApiMock.Verify(api => api.GetSlotByIdAsync(command.SlotId), Times.Once);
    }
}