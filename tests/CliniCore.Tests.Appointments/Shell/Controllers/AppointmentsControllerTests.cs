using AutoFixture;
using CliniCore.Modules.Appointments.Core.DTO;
using CliniCore.Modules.Appointments.Core.Models;
using CliniCore.Modules.Appointments.Shell.DAL;
using CliniCore.Modules.Appointments.Shell.DAL.Entities;
using CliniCore.Modules.Appointments.Shell.DTO;
using CliniCore.Tests.Shared;
using CliniCore.Tests.Shared.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace CliniCore.Tests.Appointments.Shell.Controllers;
public class AppointmentsControllerTests : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly IFixture _fixture;
    private readonly HttpClient _client;

    public AppointmentsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _fixture = new Fixture();
        _client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await TestUtils.ClearDatabaseAsync(_factory); // Clear the database before each test
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task GetUpcomingAppointments_ShouldReturnOnlyUpcomingAppointments_WhenAppointmentsExist()
    {
        // Arrange
        var upcomingAppointments = _fixture.Build<AppointmentEntity>()
            .With(a => a.Time, DateTime.UtcNow.AddDays(10))
            .With(a => a.Status, AppointmentStatus.Booked)
            .CreateMany(3);

        var upcomingCompletedAppointments = _fixture.Build<AppointmentEntity>()
            .With(a => a.Time, DateTime.UtcNow.AddDays(10))
            .With(a => a.Status, AppointmentStatus.Completed)
            .CreateMany(3);

        var upcomingCanceledAppointments = _fixture.Build<AppointmentEntity>()
            .With(a => a.Time, DateTime.UtcNow.AddDays(10))
            .With(a => a.Status, AppointmentStatus.Canceled)
            .CreateMany(3);

        var passedAppointments = _fixture.Build<AppointmentEntity>()
            .With(a => a.Time, DateTime.UtcNow.AddDays(-10))
            .With(a => a.Status, AppointmentStatus.Canceled)
            .CreateMany(3);

        var allAppointments = upcomingAppointments
            .Concat(upcomingCompletedAppointments)
            .Concat(upcomingCanceledAppointments)
            .Concat(passedAppointments);

        await TestUtils.AddToDatabaseAsync<AppointmentsDbContext, AppointmentEntity>(_factory, allAppointments);

        // Act
        var response = await _client.GetAsync("/api/v1/appointments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var appointments = await response.Content.ReadFromJsonAsync<IEnumerable<AppointmentDto>>();
        appointments.Should().NotBeNull();
        appointments.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetUpcomingAppointments_ShouldReturnEmptyList_WhenNoAppointmentsExist()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/appointments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var appointments = await response.Content.ReadFromJsonAsync<IEnumerable<AppointmentDto>>();
        appointments.Should().NotBeNull();
        appointments.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAppointmentStatus_ShouldReturnOk_WhenRequestIsValid()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var updateRequest = new UpdateStatusRequest { Status = "Completed" };


        // Pre-seed database with the appointment
        var appointment = _fixture.Build<AppointmentEntity>()
                                  .With(a => a.Id, appointmentId)
                                  .With(a => a.Status, AppointmentStatus.Booked)
                                  .Create();

        await TestUtils.AddToDatabaseAsync<AppointmentsDbContext, AppointmentEntity>(_factory, appointment);

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/v1/appointments/{appointmentId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content.ReadAsStringAsync();
        var updateResponse = JsonConvert.DeserializeObject<UpdateStatusResponse>(responseContent);

        updateResponse.Message.Should().Be("Status has been updated successfully");

        var updatedAppointment = await TestUtils.GetFromDatabaseAsync<AppointmentsDbContext, AppointmentEntity>(_factory, appointmentId);
        updatedAppointment.Status.Should().Be(AppointmentStatus.Completed);
    }

    [Fact]
    public async Task UpdateAppointmentStatus_ShouldReturnNotFound_WhenAppointmentDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var updateRequest = new UpdateStatusRequest { Status = "Completed" };

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/v1/appointments/{nonExistentId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateAppointmentStatus_ShouldReturnBadRequest_WhenStatusIsInvalid()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var updateRequest = new UpdateStatusRequest { Status = "InvalidStatus" };

        // Pre-seed database with the appointment
        var appointment = _fixture.Build<AppointmentEntity>()
                                  .With(a => a.Id, appointmentId)
                                  .With(a => a.Status, AppointmentStatus.Booked)
                                  .Create();

        await TestUtils.AddToDatabaseAsync<AppointmentsDbContext, AppointmentEntity>(_factory, appointment);

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/v1/appointments/{appointmentId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
