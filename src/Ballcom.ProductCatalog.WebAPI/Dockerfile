FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /src
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Ballcom.ProductCatalog.WebAPI/Ballcom.ProductCatalog.WebAPI.csproj", "src/Ballcom.ProductCatalog.WebAPI/"]
COPY ["Aspire/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj", "Aspire/Aspire.ServiceDefaults/"]
COPY ["src/Ballcom.ProductCatalog.Application/Ballcom.ProductCatalog.Application.csproj", "src/Ballcom.ProductCatalog.Application/"]
COPY ["src/Ballcom.ProductCatalog.Domain/Ballcom.ProductCatalog.Domain.csproj", "src/Ballcom.ProductCatalog.Domain/"]
COPY ["src/contracts/ProductCreatedEvent/Events.csproj", "src/contracts/ProductCreatedEvent/"]
COPY ["src/Ballcom.ProductCatalog.Infrastructure/Ballcom.ProductCatalog.Infrastructure.csproj", "src/Ballcom.ProductCatalog.Infrastructure/"]
RUN dotnet restore "./src/Ballcom.ProductCatalog.WebAPI/Ballcom.ProductCatalog.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Ballcom.ProductCatalog.WebAPI"
RUN dotnet build "./Ballcom.ProductCatalog.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "./Ballcom.ProductCatalog.WebAPI.csproj" -c -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ballcom.ProductCatalog.WebAPI.dll"]