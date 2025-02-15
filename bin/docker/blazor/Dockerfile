FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
EXPOSE 8080
WORKDIR /app
COPY Application.csproj ./
RUN dotnet restore Application.csproj