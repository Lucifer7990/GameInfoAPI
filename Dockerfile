# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy only project files first
COPY ["TestAPI.sln", "."]
COPY ["API/API.csproj", "API/"]
COPY ["global.json", "."]

# Restore packages
RUN dotnet restore TestAPI.sln

# Copy everything else
COPY . .

# Publish
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "API.dll"]