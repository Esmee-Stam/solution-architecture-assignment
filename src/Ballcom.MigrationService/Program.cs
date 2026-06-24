using Ballcom.CustomerService.Infrastructure.Data;
using Ballcom.Identity.Infrastructure.Data;
using Ballcom.MigrationService;
using Ballcom.ProductCatalog.Infrastructure.Data.Read;
using Ballcom.ProductCatalog.Infrastructure.Data.Write;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddDbContext<CustomerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-customer-service"));
});

var host = builder.Build();
host.Run();
