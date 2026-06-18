FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj files first (cache optimalisatie)
COPY ["src/Ballcom.Identity.WebAPI/Ballcom.Identity.WebAPI.csproj", "src/Ballcom.Identity.WebAPI/"]
COPY ["Aspire/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj", "Aspire/Aspire.ServiceDefaults/"]
COPY ["src/Ballcom.Identity.DomainServices/Ballcom.Identity.DomainServices.csproj", "src/Ballcom.Identity.DomainServices/"]
COPY ["src/Ballcom.Identity.Domain/Ballcom.Identity.Domain.csproj", "src/Ballcom.Identity.Domain/"]
COPY ["src/Ballcom.Identity.Infrastructure/Ballcom.Identity.Infrastructure.csproj", "src/Ballcom.Identity.Infrastructure/"]

# restore dependencies
RUN dotnet restore "src/Ballcom.Identity.WebAPI/Ballcom.Identity.WebAPI.csproj"

# copy everything
COPY . .

# build project
WORKDIR /src/src/Ballcom.Identity.WebAPI
RUN dotnet build "Ballcom.Identity.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ballcom.Identity.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ballcom.Identity.WebAPI.dll"]