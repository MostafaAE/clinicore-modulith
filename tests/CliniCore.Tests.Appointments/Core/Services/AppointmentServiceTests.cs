using AutoFixture;
using CliniCore.Modules.Appointments.Core.DTO;
using CliniCore.Modules.Appointments.Core.Exceptions;
using CliniCore.Modules.Appointments.Core.Models;
using CliniCore.Modules.Appointments.Core.OutputPorts;
using CliniCore.Modules.Appointments.Core.Services;
using FluentAssertions;
using Moq;

namespace CliniCore.Tests.Appointments.Core.Services;
public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly AppointmentService _appointmentsService;
    private readonly IFixture _fixture;

    public AppointmentServiceTests()
    {
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _appointmentsService = new AppointmentService(_appointmentRepositoryMock.Object);
        _fixture = new Fixture();
    }
    [Fact]
    public async Task GetUpcomingAppointmentsAsync_ShouldReturnMappedDtos_WhenAppointmentsExist()
    {
        // Arrange
        var upcomingAppointments = _fixture.CreateMany<Appointment>(3).ToList();
        _appointmentRepositoryMock.Setup(repo => repo.GetUpcomingAppointmentsAsync())
            .ReturnsAsync(upcomingAppointments);

        // Act
        var result = await _appointmentsService.GetUpcomingAppointmentsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        var resultList = result.ToList();
        for (int i = 0; i < resultList.Count; i++)
        {
            resultList[i].Id.Should().Be(upcomingAppointments[i].Id);
            resultList[i].DoctorName.Should().Be(upcomingAppointments[i].DoctorName);
            resultList[i].PatientName.Should().Be(upcomingAppointments[i].PatientName);
        }

        _appointmentRepositoryMock.Verify(repo => repo.GetUpcomingAppointmentsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetUpcomingAppointmentsAsync_ShouldReturnEmptyList_WhenNoAppointmentsExist()
    {
        // Arrange
        _appointmentRepositoryMock.Setup(repo => repo.GetUpcomingAppointmentsAsync())
            .ReturnsAsync(Enumerable.Empty<Appointment>());

        // Act
        var result = await _appointmentsService.GetUpcomingAppointmentsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _appointmentRepositoryMock.Verify(repo => repo.GetUpcomingAppointmentsAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAppointmentStatusAsync_ShouldCompleteAppointment_WhenStatusIsCompleted()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var appointment = Appointment.Create(appointmentId, Guid.NewGuid(), Guid.NewGuid(), DateTime.Now,
            Guid.NewGuid(), "", Guid.NewGuid(), "", DateTime.Now, 100, AppointmentStatus.Booked);

        _appointmentRepositoryMock
            .Setup(repo => repo.GetAppointmentByIdAsync(appointmentId))
            .ReturnsAsync(appointment);

        var command = new UpdateStatusCommand { Status = AppointmentStatus.Completed };

        // Act
        await _appointmentsService.UpdateAppointmentStatusAsync(appointmentId, command);

        // Assert
        appointment.Status.Should().Be(AppointmentStatus.Completed);
        _appointmentRepositoryMock.Verify(repo => repo.UpdateAppointmentAsync(appointment), Times.Once);
    }

    [Fact]
    public async Task UpdateAppointmentStatusAsync_ShouldCancelAppointment_WhenStatusIsCanceled()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var appointment = Appointment.Create(appointmentId, Guid.NewGuid(), Guid.NewGuid(), DateTime.Now,
            Guid.NewGuid(), "", Guid.NewGuid(), "", DateTime.Now, 100, AppointmentStatus.Booked);

        _appointmentRepositoryMock
            .Setup(repo => repo.GetAppointmentByIdAsync(appointmentId))
            .ReturnsAsync(appointment);

        var command = new UpdateStatusCommand { Status = AppointmentStatus.Canceled };

        // Act
        await _appointmentsService.UpdateAppointmentStatusAsync(appointmentId, command);

        // Assert
        appointment.Status.Should().Be(AppointmentStatus.Canceled);
        _appointmentRepositoryMock.Verify(repo => repo.UpdateAppointmentAsync(appointment), Times.Once);
    }

    [Fact]
    public async Task UpdateAppointmentStatusAsync_ShouldThrowException_WhenAppointmentNotFound()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();

        _appointmentRepositoryMock
            .Setup(repo => repo.GetAppointmentByIdAsync(appointmentId))
            .ReturnsAsync((Appointment)null);

        var command = new UpdateStatusCommand { Status = AppointmentStatus.Completed };

        // Act
        Func<Task> act = async () => await _appointmentsService.UpdateAppointmentStatusAsync(appointmentId, command);

        // Assert
        await act.Should().ThrowAsync<AppointmentNotFoundException>();
        _appointmentRepositoryMock.Verify(repo => repo.UpdateAppointmentAsync(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAppointmentStatusAsync_ShouldThrowException_WhenAppointmentHasBeenCompletedOrCanceled()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();

        var appointment = Appointment.Create(appointmentId, Guid.NewGuid(), Guid.NewGuid(), DateTime.Now,
            Guid.NewGuid(), "", Guid.NewGuid(), "", DateTime.Now, 100, AppointmentStatus.Completed);

        _appointmentRepositoryMock
            .Setup(repo => repo.GetAppointmentByIdAsync(appointmentId))
            .ReturnsAsync(appointment);

        var command = new UpdateStatusCommand { Status = AppointmentStatus.Completed };

        // Act
        Func<Task> act = async () => await _appointmentsService.UpdateAppointmentStatusAsync(appointmentId, command);

        // Assert
        await act.Should().ThrowAsync<AppointmentAlreadyCompletedOrCanceledFoundException>();
        _appointmentRepositoryMock.Verify(repo => repo.UpdateAppointmentAsync(It.IsAny<Appointment>()), Times.Never);
    }
}