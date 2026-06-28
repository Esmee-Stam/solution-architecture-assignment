FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Ballcom.CustomerService.WebAPI/Ballcom.CustomerService.WebAPI.csproj", "src/Ballcom.CustomerService.WebAPI/"]
COPY ["Aspire/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj", "Aspire/Aspire.ServiceDefaults/"]
COPY ["src/Ballcom.CustomerService.Infrastructure/Ballcom.CustomerService.Infrastructure.csproj", "src/Ballcom.CustomerService.Infrastructure/"]
COPY ["src/Ballcom.CustomerService.Application/Ballcom.CustomerService.Application.csproj", "src/Ballcom.CustomerService.Application/"]
COPY ["src/Ballcom.CustomerService.Domain/Ballcom.CustomerService.Domain.csproj", "src/Ballcom.CustomerService.Domain/"]
COPY ["src/contracts/ProductCreatedEvent/Events.csproj", "src/contracts/ProductCreatedEvent/"]
RUN dotnet restore "./src/Ballcom.CustomerService.WebAPI/Ballcom.CustomerService.WebAPI.csproj"

COPY . .

WORKDIR "/src/src/Ballcom.CustomerService.WebAPI"
RUN dotnet build "Ballcom.CustomerService.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Ballcom.CustomerService.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ballcom.CustomerService.WebAPI.dll"]