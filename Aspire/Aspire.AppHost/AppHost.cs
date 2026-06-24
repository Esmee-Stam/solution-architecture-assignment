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
var sqlCustomerService = builder.AddConnectionString("sql-customer-service");
var sqlOrderWrite = builder.AddConnectionString("sql-order-write");
var sqlOrderRead = builder.AddConnectionString("sql-order-read");
var sqlShoppingCartWrite = builder.AddConnectionString("sql-shopping-cart-write");
var sqlShoppingCartRead = builder.AddConnectionString("sql-shopping-cart-read");

// Migrations
var migration = builder.AddProject<Projects.Ballcom_MigrationService>("Migrations")
    .WithReference(sqlIdentity)
    .WithReference(sqlProductCatalogWrite)
    .WithReference(sqlProductCatalogRead)
    .WithReference(sqlCustomerService)
    .WithReference(sqlOrderWrite)
    .WithReference(sqlOrderRead)
    .WithReference(sqlShoppingCartRead)
    .WithReference(sqlShoppingCartWrite)
    .WaitFor(sqlIdentity)
    .WaitFor(sqlProductCatalogWrite)
    .WaitFor(sqlProductCatalogRead)
    .WaitFor(sqlCustomerService);
    .WaitFor(sqlProductCatalogRead)
    .WaitFor(sqlShoppingCartRead)
    .WaitFor(sqlShoppingCartWrite)
    .WaitFor(sqlOrderWrite)
    .WaitFor(sqlOrderRead);

// API's 
var identityApi = builder.AddProject<Projects.Ballcom_Identity_WebAPI>("identity-api")
    .WithReference(sqlIdentity)
    .WithReference(migration)
    .WithReference(rabbitmq)
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

var customerServiceApi = builder.AddProject<Projects.Ballcom_CustomerService_WebAPI>("customerservice-api")
    .WithReference(sqlCustomerService)
    .WithReference(migration)
    .WithReference(rabbitmq)
    .WaitFor(migration)
    .WithEnvironment("JWT__Secret", jwtSecret)
    .WithEnvironment("JWT__Issuer", jwtIssuer)
    .WithEnvironment("JWT__Audience", jwtAudience);


// Import Service
var importService = builder.AddProject<Projects.Ballcom_ImportService>("import-service")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq)
    .WaitFor(migration)
    .WithEnvironment("CsvImport__Url", builder.Configuration["CsvImport:Url"]);


var orderApi = builder.AddProject<Projects.Ballcom_Order_WebAPI>("ballcom-order-webapi")
    .WithReference(sqlOrderWrite)
    .WithReference(sqlOrderRead)
    .WithReference(sqlShoppingCartRead)
    .WithReference(sqlShoppingCartWrite)
    .WithReference(rabbitmq)
    .WithEnvironment("JWT__Secret", jwtSecret)
    .WithEnvironment("JWT__Issuer", jwtIssuer)
    .WithEnvironment("JWT__Audience", jwtAudience);

builder.Build().Run();
