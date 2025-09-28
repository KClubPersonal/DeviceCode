using DeviceCode.Application.Devices.Commands.DeleteDevice;
using DeviceCode.Domain.Entities;
using DeviceCode.Domain.Enums;
using DeviceCode.Domain.Exceptions;
using DeviceCode.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace DeviceCode.Tests.Unit.Application.Commands.DeleteDevice;
public class DeleteDeviceCommandHandlerTests
{
    private readonly Mock<IDeviceRepository> _mockRepository;
    private readonly DeleteDeviceCommandHandler _handler;

    public DeleteDeviceCommandHandlerTests()
    {
        // Setup: Para cada teste, criamos um novo mock do repositório
        // e uma nova instância do handler que queremos testar.
        _mockRepository = new Mock<IDeviceRepository>();
        _handler = new DeleteDeviceCommandHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenDeviceIsInUse_ShouldThrowDomainException()
    {
        // Arrange
        var device = Device.Create("Test", "Test");
        device.SetState(DeviceState.InUse);

        var command = new DeleteDeviceCommand(device.Id);

        // Mock
        _mockRepository.Setup(r => r.GetByIdAsync(device.Id)).ReturnsAsync(device);

        // Act
        var action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<DomainException>();

        // DeleteAsync should be never called
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDeviceIsAvailable_ShouldCallDeleteAsync()
    {
        // Arrange
        var device = Device.Create("Test", "Test"); 
        var command = new DeleteDeviceCommand(device.Id);
        _mockRepository.Setup(r => r.GetByIdAsync(device.Id)).ReturnsAsync(device);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        // DeleteAsync should be once.
        _mockRepository.Verify(r => r.DeleteAsync(device.Id), Times.Once);
    }
}
