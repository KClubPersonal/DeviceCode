using DeviceCode.Infrastructure;
using DeviceCode.Application;
using DeviceCode.API.Endpoints;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

//Application
builder.Services.AddApplication();

//Convert "InUse" to 1 (Example)
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapDeviceEndpoints();

app.Run();

// to be used by Tests.Integration
public partial class Program { }