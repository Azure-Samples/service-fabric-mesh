FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY counterService/counterService.csproj counterService/
RUN dotnet restore counterService/counterService.csproj
COPY . .
WORKDIR /src/counterService
RUN dotnet build counterService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish counterService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "counterService.dll"]