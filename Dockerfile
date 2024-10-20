FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ToDoApp.sln", "./"]
COPY ["src/ToDoApp.Api/ToDoApp.Api.csproj", "src/ToDoApp.Api/"]
COPY ["src/ToDoApp.Core/ToDoApp.Core.csproj", "src/ToDoApp.Core/"]
COPY ["src/ToDoApp.Infrastructure/ToDoApp.Infrastructure.csproj", "src/ToDoApp.Infrastructure/"]

RUN dotnet restore "src/ToDoApp.Api/ToDoApp.Api.csproj"

COPY . .

WORKDIR "/src/src/ToDoApp.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ToDoApp.Api.dll"]