FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DemoApplication.Worker/DemoApplication.Worker.csproj", "DemoApplication.Worker/"]
RUN dotnet restore "DemoApplication.Worker/DemoApplication.Worker.csproj"
COPY . .
WORKDIR "/src/DemoApplication.Worker"
RUN dotnet build "./DemoApplication.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DemoApplication.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoApplication.Worker.dll"]
