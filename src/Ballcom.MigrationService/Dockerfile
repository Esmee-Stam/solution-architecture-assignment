FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Ballcom.MigrationService/Ballcom.MigrationService.csproj", "src/Ballcom.MigrationService/"]
COPY ["Aspire/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj", "Aspire/Aspire.ServiceDefaults/"]
COPY ["src/Ballcom.Identity.Infrastructure/Ballcom.Identity.Infrastructure.csproj", "src/Ballcom.Identity.Infrastructure/"]
COPY ["src/Ballcom.Identity.DomainServices/Ballcom.Identity.DomainServices.csproj", "src/Ballcom.Identity.DomainServices/"]
COPY ["src/Ballcom.Identity.Domain/Ballcom.Identity.Domain.csproj", "src/Ballcom.Identity.Domain/"]
RUN dotnet restore "./src/Ballcom.MigrationService/Ballcom.MigrationService.csproj"

COPY . .

WORKDIR "/src/src/Ballcom.MigrationService"
RUN dotnet build "./Ballcom.MigrationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Ballcom.MigrationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ballcom.MigrationService.dll"]