FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# csproj cache layer
COPY ["src/Ballcom.ImportService/Ballcom.ImportService.csproj", "src/Ballcom.ImportService/"]
COPY ["Aspire/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj", "Aspire/Aspire.ServiceDefaults/"]
COPY ["src/contracts/ProductCreatedEvent/Events.csproj", "src/contracts/ProductCreatedEvent/"]

RUN dotnet restore "src/Ballcom.ImportService/Ballcom.ImportService.csproj"

COPY . .

WORKDIR /src/src/Ballcom.ImportService

RUN dotnet build "Ballcom.ImportService.csproj" -c Release -o /app/build
RUN dotnet publish "Ballcom.ImportService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Ballcom.ImportService.dll"]