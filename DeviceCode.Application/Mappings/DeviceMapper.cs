using DeviceCode.Domain.Entities;

namespace DeviceCode.Application.Mappings;
public static class DeviceMapper
{  
    public static DeviceDto ToDto(this Device device)
    {
        return new DeviceDto(
            device.Id,
            device.Name,
            device.Brand,
            device.State,
            device.CreationTime
        );
    }
}