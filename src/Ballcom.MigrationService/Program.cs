using Ballcom.CustomerService.Infrastructure.Data;
using Ballcom.Identity.Infrastructure.Data;
using Ballcom.MigrationService;
using Ballcom.Order.Infrastructure.Data.Read;
using Ballcom.Order.Infrastructure.Data.Write;
using Ballcom.ProductCatalog.Infrastructure.Data.Read;
using Ballcom.ProductCatalog.Infrastructure.Data.Write;
using Microsoft.EntityFrameworkCore;
using Ballcom.Payment.Infrastructure.Data.EventStore;
using Ballcom.Payment.Infrastructure.Data.Read;
using Ballcom.Order.Infrastructure.Data;
using Ballcom.Warehouse.Infrastructure.Data.Read;
using Ballcom.Warehouse.Infrastructure.Data.Write;
using Ballcom.Shipment.Infrastructure.Data.Read;
using Ballcom.Shipment.Infrastructure.Data.Write;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

// Database contexts for migrations
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-identity"));
});

builder.Services.AddDbContext<ProductCatalogReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-product-catalog-read"));
});

builder.Services.AddDbContext<ProductCatalogWriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-product-catalog-write"));
});
builder.Services.AddDbContext<PaymentEventStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-payment-eventstore"));
});

builder.Services.AddDbContext<PaymentReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-payment-read"));
});

builder.Services.AddDbContext<CustomerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-customer-service"));
});

builder.Services.AddDbContext<OrderWriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-order-write"));
});

builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-shopping-cart"));
});


builder.Services.AddDbContext<OrderReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-order-read"));
});


builder.Services.AddDbContext<WarehouseWriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-warehouse-write"));
});

builder.Services.AddDbContext<WarehouseReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-warehouse-read"));
});

builder.Services.AddDbContext<ShipmentWriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-shipment-write"));
});

builder.Services.AddDbContext<ShipmentReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-shipment-read"));
});

var host = builder.Build();
host.Run();
