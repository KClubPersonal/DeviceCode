using DeviceCode.Domain.Enums;
using DeviceCode.Domain.Exceptions;
using DeviceCode.Domain.Interfaces;
using MediatR;

namespace DeviceCode.Application.Devices.Commands.DeleteDevice;
public class DeleteDeviceCommandHandler(IDeviceRepository deviceRepository)
    : IRequestHandler<DeleteDeviceCommand>
{
    public async Task Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.Id);

        if (device is null)
        {
            return;
        }

        //Here we could have a class to validate whether it can be deleted directly in the Domain, but it would be unnecessary complexity at this moment.
        if (device.State == DeviceState.InUse)
        {
            throw new DomainException("Cannot delete a device that is currently 'InUse'.");
        }

        await deviceRepository.DeleteAsync(device.Id);
    }
}