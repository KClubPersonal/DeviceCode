using DeviceCode.Domain.Interfaces;
using DeviceCode.Infrastructure.Persistence.MongoDb.Mappings;
using DeviceCode.Infrastructure.Persistence.MongoDb.Repositories;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceCode.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        DeviceMap.Configure();

        services.AddSingleton<IMongoClient>(_ =>
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            return new MongoClient(connectionString);
        });

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var databaseName = new MongoUrl(configuration.GetConnectionString("MongoDb")).DatabaseName;
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new InvalidOperationException("MongoDB database name must be specified in the connection string.");
            }
            return client.GetDatabase(databaseName);
        });

        services.AddScoped<IDeviceRepository, DeviceRepository>();

        return services;
    }
}
