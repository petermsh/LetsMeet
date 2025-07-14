FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY LetsMeet.Api/LetsMeet.Api.csproj LetsMeet.Api/LetsMeet.Api.csproj
COPY LetsMeet.Application/LetsMeet.Application.csproj LetsMeet.Application/LetsMeet.Application.csproj
COPY LetsMeet.Domain/LetsMeet.Domain.csproj LetsMeet.Domain/LetsMeet.Domain.csproj
COPY LetsMeet.Infrastructure/LetsMeet.Infrastructure.csproj LetsMeet.Infrastructure/LetsMeet.Infrastructure.csproj

RUN dotnet restore LetsMeet.Api

COPY . .
RUN dotnet publish LetsMeet.Api -c release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ARG BUILD_VERSION
ENV BUILD_VERSION $BUILD_VERSION

ENV ASPNETCORE_ENVIRONMENT=Development

ENV ASPNETCORE_URLS=http://+:5010
ENV ASPNETCORE_URLS=http://+:5011

ENTRYPOINT ["dotnet", "LetsMeet.Api.dll"]
