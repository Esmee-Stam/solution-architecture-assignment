using Ballcom.CustomerService.Application;
using Ballcom.CustomerService.Application.Services;
using Ballcom.CustomerService.Infrastructure.Data;
using Ballcom.CustomerService.Infrastructure.Messaging;
using Ballcom.CustomerService.Infrastructure.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultAuthentication();
// Add services to the container.

builder.Services.AddDbContext<CustomerServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-customer-service"));
});

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<SyncCustomerService>();

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("customer-service", false));

    x.AddConsumer<CustomerImportedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
