using Ballcom.ImportService.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Events.CustomerServiceEvents;
using MassTransit;
using System.Globalization;

namespace Ballcom.ImportService;

public class Worker(IHttpClientFactory httpClientFactory, ILogger<Worker> logger, IBus bus, IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ImportCustomers(stoppingToken);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task ImportCustomers(CancellationToken stoppingToken)
    {
        var url = configuration["CsvImport:Url"];

        using var httpClient = httpClientFactory.CreateClient("GitHubClient");
        using var stream = await httpClient.GetStreamAsync(url, stoppingToken);
        using var reader = new StreamReader(stream);

        using var writer = new StringWriter();

        string? line;
        bool isHeader = true;

        while ((line = await reader.ReadLineAsync(stoppingToken)) != null)
        {
            line = line.Trim();

            if (!isHeader && line.StartsWith('"') && line.EndsWith('"'))
            {
                line = line.Substring(1, line.Length - 2).Replace("\"\"", "\"");
            }

            await writer.WriteLineAsync(line);
            isHeader = false;
        }

        using var csvReader = new StringReader(writer.ToString());

        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { 
            Delimiter = "," 
        };

        using var csv = new CsvReader(csvReader, config);

        var records = csv.GetRecords<ExternalCustomer>().ToList();

        foreach (var record in records)
        {
            await bus.Publish(new CustomerImportedEvent(
                record.FirstName, 
                record.LastName, 
                record.CompanyName, 
                record.PhoneNumber, 
                record.Address, 
                null
            ));
        }
    }
}