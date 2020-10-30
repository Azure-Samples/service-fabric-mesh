FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-nanoserver-1709 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 as build
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