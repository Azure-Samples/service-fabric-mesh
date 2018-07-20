FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 20009

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY WebFrontEnd/WebFrontEnd.csproj WebFrontEnd/
RUN dotnet restore WebFrontEnd/WebFrontEnd.csproj
COPY . .
WORKDIR /src/WebFrontEnd
RUN dotnet build WebFrontEnd.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebFrontEnd.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebFrontEnd.dll"]