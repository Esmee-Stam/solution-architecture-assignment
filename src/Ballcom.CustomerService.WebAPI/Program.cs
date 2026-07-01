using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Application.Queries.GetCustomerById;
using Ballcom.CustomerService.Application.Queries.GetCustomerByPhoneNumber;
using Ballcom.CustomerService.Application.Queries.GetCustomerOrders;
using Ballcom.CustomerService.Application.Queries.GetCustomerOverview;
using Ballcom.CustomerService.Application.Queries.GetCustomerShipments;
using Ballcom.CustomerService.Application.Queries.GetCustomers;
using Ballcom.CustomerService.Application.Services;
using Ballcom.CustomerService.Infrastructure.Data;
using Ballcom.CustomerService.Infrastructure.Messaging;
using Ballcom.CustomerService.Infrastructure.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultAuthentication();

builder.Services.AddControllers();

builder.Services.AddDbContext<CustomerServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-customer-service"));
});

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<SyncCustomerService>();

// Internal projection update handler. CustomerService remains read-only for external HTTP users.

builder.Services.AddScoped<GetCustomersHandler>();
builder.Services.AddScoped<GetCustomerByIdHandler>();
builder.Services.AddScoped<GetCustomerByPhoneNumberHandler>();
builder.Services.AddScoped<GetCustomerOrdersHandler>();
builder.Services.AddScoped<GetCustomerShipmentsHandler>();
builder.Services.AddScoped<GetCustomerOverviewHandler>();

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("customer", false));

    x.AddConsumer<CustomerImportedConsumer>();
    x.AddConsumer<OrderPlacedConsumer>();
    x.AddConsumer<OrderStatusChangedConsumer>();
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
