FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ToDoApp.Api/ToDoApp.Api.csproj ToDoApp.Api/
COPY ToDoApp.Core/ToDoApp.Core.csproj ToDoApp.Core/
COPY ToDoApp.Infrastructure/ToDoApp.Infrastructure.csproj ToDoApp.Infrastructure/

RUN dotnet restore ToDoApp.Api/ToDoApp.Api.csproj

COPY . .

WORKDIR /src/ToDoApp.Api
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ToDoApp.Api.dll"]