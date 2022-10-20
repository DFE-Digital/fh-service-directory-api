# Base Image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000

# Copy Solution File to support Multi-Project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY fh-service-directory-api.sln ./

# Copy Dependencies
COPY ["src/FamilyHubs.ServiceDirectoryApi.Api/FamilyHubs.ServiceDirectoryApi.Api.csproj", "src/FamilyHubs.ServiceDirectoryApi.Api/"]
COPY ["src/FamilyHubs.ServiceDirectoryApi.Core/FamilyHubs.ServiceDirectoryApi.Core.csproj", "src/FamilyHubs.ServiceDirectoryApi.Core/"]
COPY ["src/FamilyHubs.ServiceDirectoryApi.Infrastructure/FamilyHubs.ServiceDirectoryApi.Infrastructure.csproj", "src/FamilyHubs.ServiceDirectoryApi.Infrastructure/"]

# Restore Project
RUN dotnet restore "src/FamilyHubs.ServiceDirectoryApi.Api/FamilyHubs.ServiceDirectoryApi.Api.csproj"

# Copy Everything
COPY . .

# Build
WORKDIR "/src/src/FamilyHubs.ServiceDirectoryApi.Api"
RUN dotnet build "FamilyHubs.ServiceDirectoryApi.Api.csproj" -c Release -o /app/build

# publish
FROM build AS publish
WORKDIR "/src/src/FamilyHubs.ServiceDirectoryApi.Api"
RUN dotnet publish "FamilyHubs.ServiceDirectoryApi.Api.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FamilyHubs.ServiceDirectoryApi.Api.dll"]
