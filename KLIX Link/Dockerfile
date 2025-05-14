# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KLIX Link/KLIX Link.API.csproj", "KLIX Link/"]
#COPY ["KLIX Link.Core/KLIX Link.Core.csproj", "KLIX Link Core/"]
#COPY ["KLIX Link.Service/KLIX Link.Service.csproj", "KLIX Link Service/"]
#COPY ["KLIX Link.Data/KLIX Link.Data.csproj", "KLIX Link.Data/"]
RUN dotnet restore "./KLIX Link/KLIX Link.API.csproj"
COPY . .
WORKDIR "/src/KLIX Link"
RUN dotnet build "./KLIX Link.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KLIX Link.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KLIX Link.API.dll"]