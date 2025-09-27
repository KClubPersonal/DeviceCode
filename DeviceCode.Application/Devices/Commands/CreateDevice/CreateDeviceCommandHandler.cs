using DeviceCode.Domain.Entities;
using DeviceCode.Domain.Interfaces;
using MediatR;

namespace DeviceCode.Application.Devices.Commands.CreateDevice;
public class CreateDeviceCommandHandler(IDeviceRepository deviceRepository) : IRequestHandler<CreateDeviceCommand, Guid>
{
    public async Task<Guid> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        // Here I used the factory entity to ensure consistency.
        var device = Device.Create(request.Name, request.Brand);

        await deviceRepository.AddAsync(device);

        return device.Id;
    }
}
