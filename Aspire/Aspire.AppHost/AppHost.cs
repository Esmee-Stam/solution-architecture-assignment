using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var jwtSecret = configuration["JWT:Secret"];
var jwtIssuer = configuration["JWT:Issuer"];
var jwtAudience = configuration["JWT:Audience"];

// RabbitMQ
var rabbitMqConnectionString = configuration.GetConnectionString("messaging");

var rabbitmq = builder.AddConnectionString("messaging");

// SQL databases per microservice
var sqlIdentity = builder.AddConnectionString("sql-identity");
var sqlProductCatalog = builder.AddConnectionString("sql-product-catalog");

// Migrations
var migration = builder.AddProject<Projects.Ballcom_MigrationService>("Migrations")
    .WithEnvironment("ConnectionStrings__sql-identity", configuration.GetConnectionString("sql-identity"))
    .WithEnvironment("ConnectionStrings__sql-product-catalog", configuration.GetConnectionString("sql-product-catalog"))
    .WaitFor(sqlIdentity)
    .WaitFor(sqlProductCatalog);

// API's 
var identityApi = builder.AddProject<Projects.Ballcom_Identity_WebAPI>("identity-api")
    .WithReference(sqlIdentity)
    .WithReference(migration)
    .WaitFor(migration)
    .WithEnvironment("JWT__Secret", jwtSecret)
    .WithEnvironment("JWT__Issuer", jwtIssuer)
    .WithEnvironment("JWT__Audience", jwtAudience);

var productCatalogApi = builder.AddProject<Projects.Ballcom_ProductCatalog_WebAPI>("productcatalog-api")
    .WithReference(sqlProductCatalog)
    .WithReference(migration)
    .WaitFor(migration)
    .WithEnvironment("JWT__Secret", jwtSecret)
    .WithEnvironment("JWT__Issuer", jwtIssuer)
    .WithEnvironment("JWT__Audience", jwtAudience);

builder.Build().Run();
