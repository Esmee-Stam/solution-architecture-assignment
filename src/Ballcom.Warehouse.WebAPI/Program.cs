using Ballcom.Warehouse.Application.Commands.CreateWarehouseOrderFromPaidOrder;
using Ballcom.Warehouse.Application.Commands.PackWarehouseOrder;
using Ballcom.Warehouse.Application.Commands.PickWarehouseOrder;
using Ballcom.Warehouse.Application.Interfaces;
using Ballcom.Warehouse.Application.Queries.GetAllWarehouseOrders;
using Ballcom.Warehouse.Application.Queries.GetWarehouseOrderById;
using Ballcom.Warehouse.Application.Queries.GetWarehouseOrderByOrderId;
using Ballcom.Warehouse.Infrastructure.Data.Read;
using Ballcom.Warehouse.Infrastructure.Data.Write;
using Ballcom.Warehouse.Infrastructure.Messaging;
using Ballcom.Warehouse.Infrastructure.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultAuthentication();

builder.Services.AddControllers();

builder.Services.AddDbContext<WarehouseWriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-warehouse-write"));
});

builder.Services.AddDbContext<WarehouseReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-warehouse-read"));
});

builder.Services.AddScoped<IWarehouseOrderWriteRepository, WarehouseOrderWriteRepository>();
builder.Services.AddScoped<IWarehouseOrderReadRepository, WarehouseOrderReadRepository>();

builder.Services.AddScoped<CreateWarehouseOrderFromPaidOrderHandler>();
builder.Services.AddScoped<PickWarehouseOrderHandler>();
builder.Services.AddScoped<PackWarehouseOrderHandler>();

builder.Services.AddScoped<GetAllWarehouseOrdersHandler>();
builder.Services.AddScoped<GetWarehouseOrderByIdHandler>();
builder.Services.AddScoped<GetWarehouseOrderByOrderIdHandler>();

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("warehouse", false));

    x.AddConsumer<PaidOrderConsumer>();
    x.AddConsumer<WarehouseOrderCreatedConsumer>();
    x.AddConsumer<WarehouseOrderPickedConsumer>();
    x.AddConsumer<WarehouseOrderPackedConsumer>();

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
