using System.Text.RegularExpressions;
using DeviceCode.Domain.Entities;
using DeviceCode.Domain.Enums;
using DeviceCode.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DeviceCode.Infrastructure.Persistence.MongoDb.Repositories;
public class DeviceRepository(IMongoDatabase database) : IDeviceRepository
{
    // The collection is named "devices" in the database.
    private readonly IMongoCollection<Device> _devices = database.GetCollection<Device>("devices");

    public async Task AddAsync(Device device)
    {
        await _devices.InsertOneAsync(device);
    }

    public async Task<Device?> GetByIdAsync(Guid id)
    {
        return await _devices.Find(d => d.Id == id).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        return await _devices.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetByBrandAsync(string brand)
    {
        var filter = Builders<Device>.Filter.Regex(
            d => d.Brand,
            new BsonRegularExpression(Regex.Escape(brand), "i")
        );

        return await _devices.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Device>> GetByStateAsync(DeviceState state)
    {
        var filter = Builders<Device>.Filter.Eq(d => d.State, state);
        return await _devices.Find(filter).ToListAsync();
    }

    public async Task UpdateAsync(Device device)
    {
        await _devices.ReplaceOneAsync(d => d.Id == device.Id, device);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _devices.DeleteOneAsync(d => d.Id == id);
    }
}
