using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Application.Commands.CreateCustomer;
using Ballcom.CustomerService.Application.Commands.UpdateCustomer;
using Ballcom.CustomerService.Application.Commands.UpsertImportedCustomer;
using Ballcom.CustomerService.Application.Queries.GetCustomerById;
using Ballcom.CustomerService.Application.Queries.GetCustomerByPhoneNumber;
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

builder.Services.AddScoped<CreateCustomerHandler>();
builder.Services.AddScoped<UpdateCustomerHandler>();
builder.Services.AddScoped<UpsertImportedCustomerHandler>();

builder.Services.AddScoped<GetCustomersHandler>();
builder.Services.AddScoped<GetCustomerByIdHandler>();
builder.Services.AddScoped<GetCustomerByPhoneNumberHandler>();

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("customer", false));

    x.AddConsumer<CustomerImportedConsumer>();

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
