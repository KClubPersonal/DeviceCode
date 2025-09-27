using DeviceCode.Application.Mappings;
using DeviceCode.Domain.Entities;
using DeviceCode.Domain.Interfaces;
using MediatR;

namespace DeviceCode.Application.Devices.Queries.GetDevicesByFilter;
public class GetDevicesByFilterQueryHandler(IDeviceRepository deviceRepository)
    : IRequestHandler<GetDevicesByFilterQuery, IEnumerable<DeviceDto>>
{
    public async Task<IEnumerable<DeviceDto>> Handle(GetDevicesByFilterQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Device> devices;

        if (!string.IsNullOrWhiteSpace(request.Brand))
        {
            devices = await deviceRepository.GetByBrandAsync(request.Brand);
        }
        else if (request.State.HasValue)
        {
            devices = await deviceRepository.GetByStateAsync(request.State.Value);
        }
        else
        {
            devices = await deviceRepository.GetAllAsync();
        }

        return devices.Select(DeviceMapper.ToDto);
    }
}
