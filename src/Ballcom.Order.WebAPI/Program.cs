using Ballcom.Order.Application.Commands.AddCartItem;
using Ballcom.Order.Application.Commands.CheckoutCart;
using Ballcom.Order.Application.Commands.RemoveCartItem;
using Ballcom.Order.Application.Interfaces;
using Ballcom.Order.Application.Queries.GetOrderById;
using Ballcom.Order.Application.Queries.GetOrdersByCustomerId;
using Ballcom.Order.Application.Queries.GetShoppingCartByCustomerId;
using Ballcom.Order.Infrastructure.Data.Read;
using Ballcom.Order.Infrastructure.Data.Write;
using Ballcom.Order.Infrastructure.Integration;
using Ballcom.Order.Infrastructure.Messaging;
using Ballcom.Order.Infrastructure.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderWriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-order-write"));
});

builder.Services.AddDbContext<ShoppingCartWriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-shopping-cart-write"));
});


builder.Services.AddDbContext<OrderReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-order-read"));
});

builder.Services.AddDbContext<ShoppingCartReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-shopping-cart-read"));
});

// Add Repositories and Handlers to the scope
builder.Services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();
builder.Services.AddScoped<IShoppingCartWriteRepository, ShoppingCartWriteRepository>();
builder.Services.AddScoped<IShoppingCartReadRepository, ShoppingCartReadRepository>();
builder.Services.AddScoped<IEventPublisher, NoOpEventPublisher>();

builder.Services.AddScoped<AddCartItemHandler>();
builder.Services.AddScoped<RemoveCartItemHandler>();
builder.Services.AddScoped<CheckoutCartHandler>();
builder.Services.AddScoped<GetShoppingCartByCustomerIdHandler>();
builder.Services.AddScoped<GetOrderByIdHandler>();
builder.Services.AddScoped<GetOrdersByCustomerIdHandler>();

// TODO: Gebruik maken van RabbitMQ hier
builder.Services.AddMassTransit(options =>
{
    // Hier plaats je de consumer, kijk naar product catalog voor voorbeeld
    options.AddConsumer<ProductAddedToCartConsumer>();

    options.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));

        cfg.ConfigureEndpoints(context);
    });
});

// Dit verwijderen als RabbitMQ werkend is
builder.Services.AddHttpClient<IProductCatalogClient, ProductCatalogHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ProductCatalog:BaseUrl"] ?? "https://localhost:7001");
});

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
