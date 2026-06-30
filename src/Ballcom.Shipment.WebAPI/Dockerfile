FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Ballcom.Shipment.WebAPI/Ballcom.Shipment.WebAPI.csproj", "src/Ballcom.Shipment.WebAPI/"]
COPY ["Aspire/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj", "Aspire/Aspire.ServiceDefaults/"]
COPY ["src/Ballcom.Shipment.Application/Ballcom.Shipment.Application.csproj", "src/Ballcom.Shipment.Application/"]
COPY ["src/Ballcom.Shipment.Domain/Ballcom.Shipment.Domain.csproj", "src/Ballcom.Shipment.Domain/"]
COPY ["src/contracts/ProductCreatedEvent/Events.csproj", "src/contracts/ProductCreatedEvent/"]
COPY ["src/Ballcom.Shipment.Infrastructure/Ballcom.Shipment.Infrastructure.csproj", "src/Ballcom.Shipment.Infrastructure/"]
RUN dotnet restore "./src/Ballcom.Shipment.WebAPI/Ballcom.Shipment.WebAPI.csproj"

COPY . .

WORKDIR "/src/src/Ballcom.Shipment.WebAPI"
RUN dotnet build "./Ballcom.Shipment.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ballcom.Shipment.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ballcom.Shipment.WebAPI.dll"]