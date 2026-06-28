FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Ballcom.Payment.WebAPI/Ballcom.Payment.WebAPI.csproj", "src/Ballcom.Payment.WebAPI/"]
COPY ["Aspire/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj", "Aspire/Aspire.ServiceDefaults/"]
COPY ["src/Ballcom.Payment.Application/Ballcom.Payment.Application.csproj", "src/Ballcom.Payment.Application/"]
COPY ["src/Ballcom.Payment.Domain/Ballcom.Payment.Domain.csproj", "src/Ballcom.Payment.Domain/"]
COPY ["src/contracts/ProductCreatedEvent/Events.csproj", "src/contracts/ProductCreatedEvent/"]
COPY ["src/Ballcom.Payment.Infrastructure/Ballcom.Payment.Infrastructure.csproj", "src/Ballcom.Payment.Infrastructure/"]
RUN dotnet restore "./src/Ballcom.Payment.WebAPI/Ballcom.Payment.WebAPI.csproj"

COPY . .

WORKDIR "/src/src/Ballcom.Payment.WebAPI"
RUN dotnet build "Ballcom.Payment.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Ballcom.Payment.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ballcom.Payment.WebAPI.dll"]