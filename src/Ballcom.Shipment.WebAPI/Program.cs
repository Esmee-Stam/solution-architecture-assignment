using Ballcom.Shipment.Application.Commands.CreateShipmentFromPackedWarehouseOrder;
using Ballcom.Shipment.Application.Commands.DeliverShipment;
using Ballcom.Shipment.Application.Commands.DispatchShipment;
using Ballcom.Shipment.Application.Interfaces;
using Ballcom.Shipment.Application.Queries.GetAllShipments;
using Ballcom.Shipment.Application.Queries.GetShipmentById;
using Ballcom.Shipment.Application.Queries.GetShipmentByOrderId;
using Ballcom.Shipment.Application.Queries.GetShipmentByTrackingNumber;
using Ballcom.Shipment.Infrastructure.Data.Read;
using Ballcom.Shipment.Infrastructure.Data.Write;
using Ballcom.Shipment.Infrastructure.Messaging;
using Ballcom.Shipment.Infrastructure.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultAuthentication();

builder.Services.AddControllers();

builder.Services.AddDbContext<ShipmentWriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-shipment-write"));
});

builder.Services.AddDbContext<ShipmentReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-shipment-read"));
});

builder.Services.AddScoped<IShipmentWriteRepository, ShipmentWriteRepository>();
builder.Services.AddScoped<IShipmentReadRepository, ShipmentReadRepository>();

builder.Services.AddScoped<CreateShipmentFromPackedWarehouseOrderHandler>();
builder.Services.AddScoped<DispatchShipmentHandler>();
builder.Services.AddScoped<DeliverShipmentHandler>();

builder.Services.AddScoped<GetAllShipmentsHandler>();
builder.Services.AddScoped<GetShipmentByIdHandler>();
builder.Services.AddScoped<GetShipmentByOrderIdHandler>();
builder.Services.AddScoped<GetShipmentByTrackingNumberHandler>();

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("shipment", false));

    x.AddConsumer<WarehouseOrderPackedConsumer>();
    x.AddConsumer<ShipmentCreatedConsumer>();
    x.AddConsumer<ShipmentDispatchedConsumer>();
    x.AddConsumer<ShipmentDeliveredConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConnectionString = builder.Configuration.GetConnectionString("messaging")
            ?? "amqp://guest:guest@localhost:5672/";

        cfg.Host(new Uri(rabbitMqConnectionString));
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
