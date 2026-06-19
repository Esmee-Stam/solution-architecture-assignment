using Ballcom.Identity.Infrastructure.Data;
using Ballcom.MigrationService;
using Ballcom.ProductCatalog.Infrastructure.Data;
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

builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-product-catalog"));
});

var host = builder.Build();
host.Run();
