using Ballcom.ProductCatalog.DomainServices.IRepository;
using Ballcom.ProductCatalog.Infrastructure.Data;
using Ballcom.ProductCatalog.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddDefaultAuthentication();
// Add services to the container.

builder.Services.AddDbContext<ProductCatalogDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql-product-catalog"));
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Add repositories and sercices to the scope.
builder.Services.AddScoped<IProductRepository, ProductRepository>();

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
