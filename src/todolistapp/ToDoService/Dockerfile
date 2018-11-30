FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-sac2016 AS base
WORKDIR /app
EXPOSE 20008

FROM microsoft/dotnet:2.1-sdk-nanoserver-sac2016 AS build
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