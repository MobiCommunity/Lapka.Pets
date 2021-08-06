FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY Lapka.Pets.Api/Lapka.Pets.Api.csproj Lapka.Pets.Api/Lapka.Pets.Api.csproj
COPY Lapka.Pets.Application/Lapka.Pets.Application.csproj Lapka.Pets.Application/Lapka.Pets.Application.csproj
COPY Lapka.Pets.Core/Lapka.Pets.Core.csproj Lapka.Pets.Core/Lapka.Pets.Core.csproj
COPY Lapka.Pets.Infrastructure/Lapka.Pets.Infrastructure.csproj Lapka.Pets.Infrastructure/Lapka.Pets.Infrastructure.csproj
RUN dotnet restore Lapka.Pets.Api

COPY . .
RUN dotnet publish Lapka.Pets.Api -c release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS http://*:5001
ENV ASPNETCORE_ENVIRONMENT kubernetes

EXPOSE 5001

ENTRYPOINT dotnet Lapka.Pets.Api.dll