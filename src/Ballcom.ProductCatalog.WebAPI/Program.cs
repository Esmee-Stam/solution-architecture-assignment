using Ballcom.ProductCatalog.Application.Commands.CreateProduct;
using Ballcom.ProductCatalog.Application.Queries.GetAllProducts;
using Ballcom.ProductCatalog.Application.Queries.GetProductById;
using Ballcom.ProductCatalog.Infrastructure.Data.Read;
using Ballcom.ProductCatalog.Infrastructure.Data.Write;
using Ballcom.ProductCatalog.Infrastructure.Messaging;
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
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add the handlers to the scope.
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
