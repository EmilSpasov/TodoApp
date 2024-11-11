# Use the official .NET 8 SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file and project files
COPY TodoApp.sln ./
COPY TodoApp.Api/TodoApp.Api.csproj ./TodoApp.Api/
COPY TodoApp.Core/TodoApp.Core.csproj ./TodoApp.Core/
COPY TodoApp.Infrastructure/TodoApp.Infrastructure.csproj ./TodoApp.Infrastructure/
COPY TodoApp.Tests/TodoApp.Tests.csproj ./TodoApp.Tests/
RUN dotnet restore

# Copy the remaining source code and build the application
COPY . ./
WORKDIR /app/TodoApp.Api
RUN dotnet publish -c Release -o out

# Use the official .NET 8 runtime image as a runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install curl
RUN apt-get update && apt-get install -y curl

# Create the /app/data directory
RUN mkdir -p /app/data

# Copy the published output from the build stage
COPY --from=build /app/TodoApp.Api/out .

# Expose port 8080
EXPOSE 8080

# Set the entry point for the application
ENTRYPOINT ["dotnet", "TodoApp.Api.dll"]

