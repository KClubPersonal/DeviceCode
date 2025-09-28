using DeviceCode.Application.Devices.Queries.GetDevicesByFilter;
using DeviceCode.Application.Dtos;
using DeviceCode.Domain.Entities;
using DeviceCode.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace DeviceCode.Tests.Unit.Application.Queries.GetDevicesByFilter;
public class GetDevicesByFilterQueryHandlerTests
{
    private readonly Mock<IDeviceRepository> _mockRepository;
    private readonly GetDevicesByFilterQueryHandler _handler;

    public GetDevicesByFilterQueryHandlerTests()
    {
        _mockRepository = new Mock<IDeviceRepository>();
        _handler = new GetDevicesByFilterQueryHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenNoFilterIsProvided_ShouldCallGetAllAsyncAndReturnMappedDtos()
    {
        // Arrange 
        var query = new GetDevicesByFilterQuery(null, null);       
        var devicesFromRepo = new List<Device>
        {
            Device.Create("Device 1", "Brand A"),
            Device.Create("Device 2", "Brand B")
        };

        // Mock
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(devicesFromRepo);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert (Verificação)
        result.Should().HaveCount(devicesFromRepo.Count);
        result.First().Should().BeOfType<DeviceDto>();
        result.First().Name.Should().Be("Device 1");

        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        _mockRepository.Verify(r => r.GetByBrandAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenBrandFilterIsProvided_ShouldCallGetByBrandAsync()
    {
        // Arrange
        var brandToFilter = "Apple";
        var query = new GetDevicesByFilterQuery(brandToFilter, null);
        var devicesFromRepo = new List<Device> { Device.Create("iPhone", brandToFilter) };
        _mockRepository.Setup(r => r.GetByBrandAsync(brandToFilter)).ReturnsAsync(devicesFromRepo);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        _mockRepository.Verify(r => r.GetByBrandAsync(brandToFilter), Times.Once);
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Never);
    }
}