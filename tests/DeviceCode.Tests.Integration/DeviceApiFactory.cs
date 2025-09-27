using DeviceCode.API.Endpoints;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace DeviceCode.Tests.Integration;
public class DeviceApiFactory : WebApplicationFactory<CreateDeviceRequest>, IAsyncLifetime
{
    private readonly IContainer _mongo = new ContainerBuilder()
        .WithImage("mongo:6.0")
        .WithPortBinding(27017, true) 
        .WithCommand("mongod", "--bind_ip_all", "--noauth")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var host = _mongo.Hostname;
            var port = _mongo.GetMappedPublicPort(27017);
            var url = new MongoUrlBuilder($"mongodb://{host}:{port}")
            {
                DatabaseName = "DeviceTestDb"
            }.ToString();

            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:MongoDb"] = url
            });
        });
    }

    public Task InitializeAsync() => _mongo.StartAsync();
    public new Task DisposeAsync() => _mongo.StopAsync();
}