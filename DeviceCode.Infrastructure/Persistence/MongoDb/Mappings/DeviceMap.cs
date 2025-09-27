using DeviceCode.Domain.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DeviceCode.Infrastructure.Persistence.MongoDb.Mappings;
public static class DeviceMap
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<Device>(cm =>
        {
            cm.AutoMap();
            cm.MapIdProperty(d => d.Id)
              .SetSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
        });
    }
}
