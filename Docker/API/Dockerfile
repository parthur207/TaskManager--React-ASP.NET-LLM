FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto para restaurar dependências com cache
COPY ["TaskManager.API/TaskManager.API.csproj",           "TaskManager.API/"]
COPY ["TaskManager.Adapters/TaskManager.Adapters.csproj", "TaskManager.Adapters/"]
COPY ["TaskManager.Core/TaskManager.Core.csproj",         "TaskManager.Core/"]

RUN dotnet restore "TaskManager.API/TaskManager.API.csproj"

COPY . .

WORKDIR "/src/TaskManager.API"
RUN dotnet build "TaskManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.API.dll"]