FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY settingsService/settingsService.csproj settingsService/
RUN dotnet restore settingsService/settingsService.csproj
COPY . .
WORKDIR /src/settingsService
RUN dotnet build settingsService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish settingsService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "settingsService.dll"]