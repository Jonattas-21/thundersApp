FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApi/thundersApp.csproj", "WebApi/"]
RUN dotnet restore "./WebApi/thundersApp.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "./thundersApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Install dotnet-ef tool locally
RUN dotnet tool install dotnet-ef --version 8.0.0 --tool-path /app/.tools

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./thundersApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /app/.tools /root/.dotnet/tools

# Apply migrations and start the application
ENTRYPOINT ["dotnet", "thundersApp.dll"]
