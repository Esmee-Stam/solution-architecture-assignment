# Ballcom Customer Service

The Customer Service owns customer profile data for Ball.com. It stores customers imported from the CSV import worker and also exposes manual API endpoints for creating, updating and reading customers.

## Architectural role

- Bounded context: Customer Service
- Receives customer import messages through RabbitMQ/MassTransit
- Publishes customer lifecycle messages when a customer is created or updated
- Stores customer data in `CustomerServiceDB`
- Exposes REST endpoints for query and manual test operations

## Event flow

```text
ImportService
  -> CustomerImportedEvent
  -> CustomerImportedConsumer
  -> UpsertImportedCustomerHandler
  -> CustomerServiceDB
  -> CustomerCreatedEvent / CustomerUpdatedEvent
```

Manual API changes also publish `CustomerCreatedEvent` or `CustomerUpdatedEvent`.

## Main endpoints

```http
GET  /health
GET  /api/customers?pageNumber=1&pageSize=50&search=term
GET  /api/customers/{customerId}
GET  /api/customers/phone/{phoneNumber}
POST /api/customers
PUT  /api/customers/{customerId}
POST /api/customers/imported
```

`POST /api/customers/imported` is mainly for manual testing of the import/upsert behavior without waiting for the import worker.

## User secrets

The service uses the existing CustomerService connection string:

```powershell
dotnet user-secrets set "ConnectionStrings:sql-customer-service" "Server=localhost,2026;Database=CustomerServiceDB;User Id=sa;Password=SuperStrongPassword1234!;TrustServerCertificate=True;"
```

RabbitMQ is shared with the other event-driven services:

```powershell
dotnet user-secrets set "ConnectionStrings:messaging" "amqp://guest:guest@localhost:5672/"
```
