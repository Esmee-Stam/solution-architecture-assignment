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
var sqlPaymentEventStore = builder.AddConnectionString("sql-payment-eventstore");
var sqlPaymentRead = builder.AddConnectionString("sql-payment-read");
var sqlCustomerService = builder.AddConnectionString("sql-customer-service");
var sqlOrderWrite = builder.AddConnectionString("sql-order-write");
var sqlOrderRead = builder.AddConnectionString("sql-order-read");
var sqlShoppingCart= builder.AddConnectionString("sql-shopping-cart");
// Migrations
var migration = builder.AddProject<Projects.Ballcom_MigrationService>("Migrations")
    .WithReference(sqlIdentity)
    .WithReference(sqlProductCatalogWrite)
    .WithReference(sqlProductCatalogRead)
    .WithReference(sqlPaymentEventStore)
    .WithReference(sqlPaymentRead)
    .WithReference(sqlCustomerService)
    .WithReference(sqlOrderWrite)
    .WithReference(sqlOrderRead)
    .WithReference(sqlShoppingCart)
    .WaitFor(sqlIdentity)
    .WaitFor(sqlProductCatalogWrite)
    .WaitFor(sqlProductCatalogRead)
    .WaitFor(sqlPaymentEventStore)
    .WaitFor(sqlPaymentRead)
    .WaitFor(sqlCustomerService)
    .WaitFor(sqlCustomerService)
    .WaitFor(sqlProductCatalogRead)
    .WaitFor(sqlShoppingCart)
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


var paymentApi = builder.AddProject<Projects.Ballcom_Payment_WebAPI>("payment-api")
    .WithReference(sqlPaymentEventStore)
    .WithReference(sqlPaymentRead)
    .WithReference(rabbitmq)
    .WithReference(migration)
    .WithEnvironment("JWT__Secret", jwtSecret)
    .WithEnvironment("JWT__Issuer", jwtIssuer)
    .WithEnvironment("JWT__Audience", jwtAudience);

var orderApi = builder.AddProject<Projects.Ballcom_Order_WebAPI>("ballcom-order-webapi")
    .WithReference(sqlOrderWrite)
    .WithReference(sqlOrderRead)
    .WithReference(sqlShoppingCart)
    .WithReference(rabbitmq)
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
//var importService = builder.AddProject<Projects.Ballcom_ImportService>("import-service")
//    .WithReference(rabbitmq)
//    .WaitFor(rabbitmq)
//    .WaitFor(migration)
//    .WithEnvironment("CsvImport__Url", builder.Configuration["CsvImport:Url"]);

builder.Build().Run();