FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Ballcom.Warehouse.WebAPI/Ballcom.Warehouse.WebAPI.csproj", "src/Ballcom.Warehouse.WebAPI/"]
COPY ["Aspire/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj", "Aspire/Aspire.ServiceDefaults/"]
COPY ["src/Ballcom.Warehouse.Application/Ballcom.Warehouse.Application.csproj", "src/Ballcom.Warehouse.Application/"]
COPY ["src/Ballcom.Warehouse.Domain/Ballcom.Warehouse.Domain.csproj", "src/Ballcom.Warehouse.Domain/"]
COPY ["src/contracts/ProductCreatedEvent/Events.csproj", "src/contracts/ProductCreatedEvent/"]
COPY ["src/Ballcom.Warehouse.Infrastructure/Ballcom.Warehouse.Infrastructure.csproj", "src/Ballcom.Warehouse.Infrastructure/"]
RUN dotnet restore "./src/Ballcom.Warehouse.WebAPI/Ballcom.Warehouse.WebAPI.csproj"

COPY . .

WORKDIR "/src/src/Ballcom.Warehouse.WebAPI"
RUN dotnet build "./Ballcom.Warehouse.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ballcom.Warehouse.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ballcom.Warehouse.WebAPI.dll"]