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
var sqlProductCatalogWrite = builder.AddConnectionString("sql-product-catalog-write");
var sqlProductCatalogRead = builder.AddConnectionString("sql-product-catalog-read");

// Migrations
var migration = builder.AddProject<Projects.Ballcom_MigrationService>("Migrations")
    .WithReference(sqlIdentity)
    .WithReference(sqlProductCatalogWrite)
    .WithReference(sqlProductCatalogRead)
    .WaitFor(sqlIdentity)
    .WaitFor(sqlProductCatalogWrite)
    .WaitFor(sqlProductCatalogRead);

// API's 
var identityApi = builder.AddProject<Projects.Ballcom_Identity_WebAPI>("identity-api")
    .WithReference(sqlIdentity)
    .WithReference(migration)
    .WaitFor(migration)
    .WithEnvironment("JWT__Secret", jwtSecret)
    .WithEnvironment("JWT__Issuer", jwtIssuer)
    .WithEnvironment("JWT__Audience", jwtAudience);

var productCatalogApi = builder.AddProject<Projects.Ballcom_ProductCatalog_WebAPI>("productcatalog-api")
    .WithReference(sqlProductCatalogWrite)
    .WithReference(sqlProductCatalogRead)
    .WithReference(rabbitmq)
    .WithReference(migration)
    .WaitFor(migration)
    .WithEnvironment("JWT__Secret", jwtSecret)
    .WithEnvironment("JWT__Issuer", jwtIssuer)
    .WithEnvironment("JWT__Audience", jwtAudience);

builder.AddProject<Projects.Ballcom_Order_WebAPI>("ballcom-order-webapi");

builder.Build().Run();
