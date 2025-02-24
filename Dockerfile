# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy project files
COPY PoemPass/*.csproj PoemPass/

# Restore dependencies
RUN dotnet restore PoemPass/PoemPass.csproj

# Copy remaining files and publish the application
COPY . .
RUN dotnet publish "PoemPass/PoemPass.csproj" -c Release -o /publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy AS final
WORKDIR /app

# Copy published output
COPY --from=build /publish .

# Entry point for the application
ENTRYPOINT ["dotnet", "PoemPass.dll"]