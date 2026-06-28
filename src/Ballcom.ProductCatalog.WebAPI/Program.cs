using Ballcom.ProductCatalog.Application.Commands.CreateProduct;
using Ballcom.ProductCatalog.Application.Interfaces;
using Ballcom.ProductCatalog.Application.Queries.GetAllProducts;
using Ballcom.ProductCatalog.Application.Queries.GetProductById;
using Ballcom.ProductCatalog.Infrastructure.Data.Read;
using Ballcom.ProductCatalog.Infrastructure.Data.Write;
using Ballcom.ProductCatalog.Infrastructure.Messaging;
using Ballcom.ProductCatalog.Infrastructure.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddDefaultAuthentication();
// Add services to the container.

builder.Services.AddDbContext<ProductCatalogWriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-product-catalog-write"));
});

builder.Services.AddDbContext<ProductCatalogReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-product-catalog-read"));
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConnectionString = builder.Configuration.GetConnectionString("messaging")
            ?? "amqp://guest:guest@localhost:5672/";

        cfg.Host(new Uri(rabbitMqConnectionString));

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add the repositories and handlers to the scope.
builder.Services.AddScoped<IProductReadRepository, ProductReadRepository>();
builder.Services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
builder.Services.AddScoped<GetAllProductsHandler>();
builder.Services.AddScoped<GetProductByIdHandler>();
builder.Services.AddScoped<CreateProductHandler>();

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
