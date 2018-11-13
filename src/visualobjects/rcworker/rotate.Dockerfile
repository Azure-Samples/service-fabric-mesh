FROM microsoft/windowsservercore AS base
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]
RUN Invoke-WebRequest -OutFile stateful_aspnetcore_2.1.ps1 https://aka.ms/sfmesh_stateful_aspnetcore_2.1.ps1; .\stateful_aspnetcore_2.1.ps1;
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY rcworker/rcworker.csproj rcworker/
COPY rcworker/Nuget.Config ./
COPY Microsoft.ServiceFabric.6.4.592.nupkg ./
COPY Microsoft.ServiceFabric.Data.Interfaces.3.3.592.nupkg ./
COPY Microsoft.ServiceFabric.Mesh.AspNetCore.Data.1.0.592-beta.nupkg ./
COPY Microsoft.ServiceFabric.Mesh.Data.Collections.1.0.592-beta.nupkg ./
RUN dotnet restore rcworker/rcworker.csproj
COPY . .
WORKDIR /src/rcworker
RUN dotnet build rcworker.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish rcworker.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV OBJECT_ENABLE_ROTATION=true
ENTRYPOINT ["dotnet", "rcworker.dll"]
