# Use the official .NET 10.0 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY FinLayer.API/*.csproj ./FinLayer.API/
RUN dotnet restore ./FinLayer.API/FinLayer.API.csproj

# Copy the rest of the source code
COPY . .

# Build and publish the app
RUN dotnet publish ./FinLayer.API/FinLayer.API.csproj -c Release -o /out

# Use the official .NET 9.0 runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /out ./

# Expose port 8080
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

# Set the entrypoint
ENTRYPOINT ["dotnet", "FinLayer.API.dll"]