#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY . ./

ARG BUILD_CONFIGURATION=Release

RUN dotnet restore

RUN dotnet publish -c $BUILD_CONFIGURATION -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "AccountPhoneManager.API.dll"]