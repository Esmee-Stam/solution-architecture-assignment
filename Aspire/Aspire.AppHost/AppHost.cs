using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var jwtSecret = configuration["JWT:Secret"];
var jwtIssuer = configuration["JWT:Issuer"];
var jwtAudience = configuration["JWT:Audience"];

// Configure RabbitMQ container
var rabbitMqConnectionString = configuration.GetConnectionString("messaging");

var rabbitmq = builder.AddConnectionString("messaging");

// Configure SQL Server container
var sqlPassword = builder.AddParameter("sqlPassword");

var sql = builder
       .AddSqlServer("sql", password: sqlPassword)
       .WithDataVolume("AspireDataVolume")
       .WithLifetime(ContainerLifetime.Persistent)
       .WithEndpointProxySupport(proxyEnabled: false);

var identityDb = sql.AddDatabase("IdentityDB", databaseName: "IdentityDB");

// Migrations
var migration = builder.AddProject<Projects.Ballcom_MigrationService>("Migrations")
    .WithReference(identityDb)
    .WaitFor(identityDb);

// API's 
var identityApi = builder.AddProject<Projects.Ballcom_Identity_WebAPI>("identity-api")
    .WithReference(identityDb)
    .WithReference(migration)
    .WaitFor(migration)
    .WithEnvironment("JWT__Secret", jwtSecret)
    .WithEnvironment("JWT__Issuer", jwtIssuer)
    .WithEnvironment("JWT__Audience", jwtAudience)
    .WithReference(identityDb);

builder.Build().Run();
