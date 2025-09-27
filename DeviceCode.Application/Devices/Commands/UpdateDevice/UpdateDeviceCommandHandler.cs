using DeviceCode.Domain.Exceptions;
using DeviceCode.Domain.Interfaces;
using MediatR;

namespace DeviceCode.Application.Devices.Commands.UpdateDevice;
public class UpdateDeviceCommandHandler(IDeviceRepository deviceRepository)
    : IRequestHandler<UpdateDeviceCommand>
{
    public async Task Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.Id) ?? throw new DomainException($"Device with ID {request.Id} not found.");

        device.SetState(request.State);

        // Again, the responsibility lies with the Domain to validate whether it can be used.
        device.UpdateDetails(request.Name, request.Brand);
        

        await deviceRepository.UpdateAsync(device);
    }
}