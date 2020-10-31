FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 as build
WORKDIR /src
COPY web/web.csproj web/
RUN dotnet restore web/web.csproj
COPY . .
WORKDIR /src/web
RUN dotnet build web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "web.dll"]