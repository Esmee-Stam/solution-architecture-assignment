FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY *.sln .
COPY src/ ./src/
COPY Aspire/ ./Aspire/

RUN dotnet restore "src/Ballcom.MigrationService/Ballcom.MigrationService.csproj"

RUN dotnet publish "src/Ballcom.MigrationService/Ballcom.MigrationService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Ballcom.MigrationService.dll"]