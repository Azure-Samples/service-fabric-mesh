FROM microsoft/windowsservercore:1803 AS base
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]
RUN Invoke-WebRequest -OutFile stateful_aspnetcore_2.1.ps1 https://aka.ms/sfmesh_stateful_aspnetcore_2.1.ps1; .\stateful_aspnetcore_2.1.ps1;
WORKDIR /app
EXPOSE 20009

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY VotingData/VotingData.csproj VotingData/
COPY VotingData/Nuget.Config ./
COPY Microsoft.ServiceFabric.6.4.592.nupkg ./
COPY Microsoft.ServiceFabric.Data.Interfaces.3.3.592.nupkg ./
COPY Microsoft.ServiceFabric.Mesh.AspNetCore.Data.1.0.592-beta.nupkg ./
COPY Microsoft.ServiceFabric.Mesh.Data.Collections.1.0.592-beta.nupkg ./
RUN dotnet restore VotingData/VotingData.csproj
COPY . .
WORKDIR /src/VotingData
RUN dotnet build VotingData.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish VotingData.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VotingData.dll"]