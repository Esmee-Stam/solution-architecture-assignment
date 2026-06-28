using Ballcom.CustomerService.Infrastructure.Data;
using Ballcom.Identity.Infrastructure.Data;
using Ballcom.Payment.Infrastructure.Data.EventStore;
using Ballcom.Payment.Infrastructure.Data.Read;
using Ballcom.Order.Infrastructure.Data.Read;
using Ballcom.Order.Infrastructure.Data.Write;
using Ballcom.ProductCatalog.Infrastructure.Data.Read;
using Ballcom.ProductCatalog.Infrastructure.Data.Write;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Ballcom.Order.Infrastructure.Data;
using Ballcom.Warehouse.Infrastructure.Data.Read;
using Ballcom.Warehouse.Infrastructure.Data.Write;
using Ballcom.Shipment.Infrastructure.Data.Read;
using Ballcom.Shipment.Infrastructure.Data.Write;

namespace Ballcom.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime
    ) : BackgroundService
{
    public const string ActivitysourceName = "Ballcom.MigrationService.Worker";

    private static readonly ActivitySource ActivitySource = new(ActivitysourceName);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();

            // Link database contexts here
            var authContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            var productCatalogReadContext = scope.ServiceProvider.GetRequiredService<ProductCatalogReadDbContext>();
            var productCatalogWriteContext = scope.ServiceProvider.GetRequiredService<ProductCatalogWriteDbContext>();
            var paymentEventStoreContext = scope.ServiceProvider.GetRequiredService<PaymentEventStoreDbContext>();
            var paymentReadContext = scope.ServiceProvider.GetRequiredService<PaymentReadDbContext>();
            var customerServiceContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
            var orderWriteContext = scope.ServiceProvider.GetRequiredService<OrderWriteDbContext>();
            var orderReadContext = scope.ServiceProvider.GetRequiredService<OrderReadDbContext>();
            var shoppingCartContext = scope.ServiceProvider.GetRequiredService<ShoppingCartDbContext>();
            var warehouseWriteContext = scope.ServiceProvider.GetRequiredService<WarehouseWriteDbContext>();
            var warehouseReadContext = scope.ServiceProvider.GetRequiredService<WarehouseReadDbContext>();
            var shipmentWriteContext = scope.ServiceProvider.GetRequiredService<ShipmentWriteDbContext>();
            var shipmentReadContext = scope.ServiceProvider.GetRequiredService<ShipmentReadDbContext>();

            // Run the migrations
            await RunMigrationAsync(authContext, stoppingToken);
            await RunMigrationAsync(productCatalogReadContext, stoppingToken);
            await RunMigrationAsync(productCatalogWriteContext, stoppingToken);
            await RunMigrationAsync(paymentEventStoreContext, stoppingToken);
            await RunMigrationAsync(paymentReadContext, stoppingToken);
            await RunMigrationAsync(customerServiceContext, stoppingToken);
            await RunMigrationAsync(orderWriteContext, stoppingToken);
            await RunMigrationAsync(orderReadContext, stoppingToken);
            await RunMigrationAsync(shoppingCartContext, stoppingToken);
            await RunMigrationAsync(warehouseWriteContext, stoppingToken);
            await RunMigrationAsync(warehouseReadContext, stoppingToken);
            await RunMigrationAsync(shipmentWriteContext, stoppingToken);
            await RunMigrationAsync(shipmentReadContext, stoppingToken);
        } 
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task RunMigrationAsync(DbContext context, CancellationToken cancellationToken)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await context.Database.MigrateAsync(cancellationToken);
        });
    }
}
