FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY stocktickerService/stocktickerService.csproj stocktickerService/
RUN dotnet restore stocktickerService/stocktickerService.csproj
COPY . .
WORKDIR /src/stocktickerService
RUN dotnet build stocktickerService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish stocktickerService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "stocktickerService.dll"]
