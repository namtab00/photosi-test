# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/PhotoSiTest.API/PhotoSiTest.API.csproj", "src/PhotoSiTest.API/"]
COPY ["src/PhotoSiTest.Addresses/PhotoSiTest.Addresses.csproj", "src/PhotoSiTest.Addresses/"]
COPY ["src/PhotoSiTest.Contracts/PhotoSiTest.Contracts.csproj", "src/PhotoSiTest.Contracts/"]
COPY ["src/PhotoSiTest.Common/PhotoSiTest.Common.csproj", "src/PhotoSiTest.Common/"]
COPY ["src/PhotoSiTest.Orders/PhotoSiTest.Orders.csproj", "src/PhotoSiTest.Orders/"]
COPY ["src/PhotoSiTest.Products/PhotoSiTest.Products.csproj", "src/PhotoSiTest.Products/"]
COPY ["src/PhotoSiTest.Users/PhotoSiTest.Users.csproj", "src/PhotoSiTest.Users/"]
RUN dotnet restore "./src/PhotoSiTest.API/PhotoSiTest.API.csproj"
COPY . .
WORKDIR "/src/src/PhotoSiTest.API"
RUN dotnet build "./PhotoSiTest.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PhotoSiTest.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhotoSiTest.API.dll"]
