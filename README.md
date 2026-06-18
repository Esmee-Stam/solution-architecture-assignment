# Solution-Architecture-Assignment - Ball.com

## About this project
This project is a .NET 10 microservices solution consisting of multiple API's, a migration service, RabbitMQ, and a SQL Server database.

---

# Requirements

### Software
- .NET 10 SDK
- Docker Desktop
- Visual Studio (2026 version) or JetBrains Rider
- SQL Server (via Docker, version 2022-latest)
- RabbitMQ (via Docker)

---

# Build the Solution
Build the Solution in Visual Studio or in the terminal: 
```bash
dotnet build
``` 
---

# Docker 
Docker Compose is used for container-based execution.

### .env file
Create a .env file in the root or use the provided example:
```env
SQL_PASSWORD=SuperStrongPassword1234!
JWT_SECRET=MySuperStrongAndVeryLongJWTSecretKeyOfAtLeast32Characters!
JWT_ISSUER=Ballcom.IdentityServer
JWT_AUDIENCE=Ballcom.WebApis
```

### Build and start the containers
```bash
docker compose up --build -d
```

# Aspire 
Aspire is used for Development.

### Step 1: Configure User Secrets
Right click on the Aspire.AppHost project -> Manage User Secrets

Add the following JSON to User Secrets. 

Make sure the values are identical to the values defined in the `.env` file to ensure consistent configuration between Docker and Aspire:
```json
{
    "ConnectionStrings:messaging": "...",
    "Parameters:SqlPassword": "...",
    "JWT_SECRET": "...",
    "JWT_ISSUER": "...",
    "JWT_AUDIENCE": "..."
}
```

# Step 2: Start the AppHost
### Visual Studio:
Set *Aspire.AppHost* as startup project and press: 
```
F5
```

### CLI
```bash
dotnet run --project Aspire.AppHost
```

---

# Workflow
- SQL Server and RabbitMQ starts in Docker
- All APIs and Migration Service runs as .NET processes
- Services are automatically connected

# Messaging (RabbitMQ)
RabbitMQ is used as a message broker for asynchronous communication betweeen services.
It is started automatically as a Docker container when running Docker Compose.
Services can publish and consume messages via RabbitMQ to support event-driven communication and loose coupling between microservices.

# Testing
API endpoints can be tested using: 
- Postman
- Bruno
