using Ballcom.Identity.Infrastructure.Data;
using Ballcom.MigrationService;
using Ballcom.ProductCatalog.Infrastructure.Data.Read;
using Ballcom.ProductCatalog.Infrastructure.Data.Write;
using Microsoft.EntityFrameworkCore;
using Ballcom.Payment.Infrastructure.Data.EventStore;
using Ballcom.Payment.Infrastructure.Data.Read;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

// Database contexts for migrations
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-identity"));
});

builder.Services.AddDbContext<UserDbContext>(options =>
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

var host = builder.Build();
host.Run();
