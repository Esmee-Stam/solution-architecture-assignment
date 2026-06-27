using Ballcom.Payment.Application.Commands.CompletePayment;
using Ballcom.Payment.Application.Commands.FailPayment;
using Ballcom.Payment.Application.Commands.RequestPayment;
using Ballcom.Payment.Application.Interfaces;
using Ballcom.Payment.Application.Queries.GetPaymentById;
using Ballcom.Payment.Application.Queries.GetPaymentByOrderId;
using Ballcom.Payment.Application.Queries.GetPaymentEvents;
using Ballcom.Payment.Infrastructure.Data.EventStore;
using Ballcom.Payment.Infrastructure.Data.Read;
using Ballcom.Payment.Infrastructure.Messaging;
using Ballcom.Payment.Infrastructure.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddDefaultAuthentication();

builder.Services.AddControllers();

builder.Services.AddDbContext<PaymentEventStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-payment-eventstore"));
});

builder.Services.AddDbContext<PaymentReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-payment-read"));
});

builder.Services.AddScoped<IPaymentEventStoreRepository, PaymentEventStoreRepository>();
builder.Services.AddScoped<IPaymentReadRepository, PaymentReadRepository>();

builder.Services.AddScoped<RequestPaymentHandler>();
builder.Services.AddScoped<CompletePaymentHandler>();
builder.Services.AddScoped<FailPaymentHandler>();

builder.Services.AddScoped<GetPaymentByIdHandler>();
builder.Services.AddScoped<GetPaymentByOrderIdHandler>();
builder.Services.AddScoped<GetPaymentEventsHandler>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PaymentRequestedConsumer>();
    x.AddConsumer<PaymentCompletedConsumer>();
    x.AddConsumer<PaymentFailedConsumer>();
    x.AddConsumer<OrderPlacedConsumer>();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();