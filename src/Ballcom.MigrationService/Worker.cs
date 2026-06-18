using Ballcom.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            var userContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

            // Run the migrations
            await RunMigrationAsync(authContext, stoppingToken);
            await RunMigrationAsync(userContext, stoppingToken);
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
