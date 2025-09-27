using DeviceCode.Application.Dtos;
using DeviceCode.Application.Mappings;
using DeviceCode.Domain.Interfaces;
using MediatR;

namespace DeviceCode.Application.Devices.Queries.GetDeviceById;
public class GetDeviceByIdQueryHandler(IDeviceRepository deviceRepository)
    : IRequestHandler<GetDeviceByIdQuery, DeviceDto?>
{
    public async Task<DeviceDto?> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.Id);

        return device?.ToDto();
    }
}