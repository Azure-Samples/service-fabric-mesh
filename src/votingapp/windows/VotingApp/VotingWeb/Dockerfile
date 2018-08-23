FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY VotingWeb/VotingWeb.csproj VotingWeb/
RUN dotnet restore VotingWeb/VotingWeb.csproj
COPY . .
WORKDIR /src/VotingWeb
RUN dotnet build VotingWeb.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish VotingWeb.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VotingWeb.dll"]