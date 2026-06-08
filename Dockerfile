# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy solution and project files
COPY TestAPI.sln .
COPY API/API.csproj API/

# Restore dependencies with timeout
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore TestAPI.sln --no-cache

# Copy source code
COPY API/ API/

# Build and publish
RUN dotnet publish "API/API.csproj" -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "API.dll"]