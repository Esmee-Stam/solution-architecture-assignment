using Ballcom.ImportService;
using Events.CustomerServiceEvents;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient("GitHubClient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConnectionString = builder.Configuration.GetConnectionString("messaging")
            ?? "amqp://guest:guest@localhost:5672/";

        cfg.Host(new Uri(rabbitMqConnectionString));
        cfg.ConfigureEndpoints(context);
    });
});


var host = builder.Build();
host.Run();
