FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY ToDoService/ToDoService.csproj ToDoService/
RUN dotnet restore ToDoService/ToDoService.csproj
COPY . .
WORKDIR /src/ToDoService
RUN dotnet build ToDoService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ToDoService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ToDoService.dll"]