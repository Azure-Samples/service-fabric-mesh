FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 as build
WORKDIR /src
COPY worker/worker.csproj worker/
RUN dotnet restore worker/worker.csproj
COPY . .
WORKDIR /src/worker
RUN dotnet build worker.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish worker.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV OBJECT_ENABLE_ROTATION=false
ENTRYPOINT ["dotnet", "worker.dll"]