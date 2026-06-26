using Ballcom.Order.Application.Commands.CheckoutCart;
using Ballcom.Order.Application.Commands.PlaceOrder;
using Ballcom.Order.Application.Commands.RemoveCartItem;
using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Application.Queries.GetOrderById;
using Ballcom.Order.Application.Queries.GetOrdersByCustomerId;
using Ballcom.Order.Application.Services;
using Ballcom.Order.Infrastructure.Data;
using Ballcom.Order.Infrastructure.Data.Read;
using Ballcom.Order.Infrastructure.Data.Write;
using Ballcom.Order.Infrastructure.Messaging;
using Ballcom.Order.Infrastructure.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddDefaultAuthentication();

builder.Services.AddControllers();

builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-shopping-cart"));
});

builder.Services.AddDbContext<OrderWriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-order-write"));
});

builder.Services.AddDbContext<OrderReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-order-read"));
});

// Add Repositories and Handlers to the scope
builder.Services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddScoped<ShoppingCartService>();

builder.Services.AddScoped<PlaceOrderCommandHandler>();
builder.Services.AddScoped<RemoveCartItemHandler>();
builder.Services.AddScoped<CheckoutCartHandler>();
builder.Services.AddScoped<GetOrderByIdHandler>();
builder.Services.AddScoped<GetOrdersByCustomerIdHandler>();

builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<ProductAddedToCartConsumer>();
    options.AddConsumer<PaymentCompletedConsumer>();
    options.AddConsumer<OrderStatusChangedConsumer>();
   
    options.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
