FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Ballcom.Order.WebAPI/Ballcom.Order.WebApi.csproj", "src/Ballcom.Order.WebAPI/"]
COPY ["Aspire/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj", "Aspire/Aspire.ServiceDefaults/"]
COPY ["src/Ballcom.Order.Application/Ballcom.Order.Application.csproj", "src/Ballcom.Order.Application/"]
COPY ["src/Ballcom.Order.Domain/Ballcom.Order.Domain.csproj", "src/Ballcom.Order.Domain/"]
COPY ["src/contracts/ProductCreatedEvent/Events.csproj", "src/contracts/ProductCreatedEvent/"]
COPY ["src/Ballcom.Order.Infrastructure/Ballcom.Order.Infrastructure.csproj", "src/Ballcom.Order.Infrastructure/"]
RUN dotnet restore "./src/Ballcom.Order.WebAPI/Ballcom.Order.WebApi.csproj"

COPY . .

WORKDIR "/src/src/Ballcom.Order.WebAPI"
RUN dotnet build "./Ballcom.Order.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ballcom.Order.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ballcom.Order.WebApi.dll"]