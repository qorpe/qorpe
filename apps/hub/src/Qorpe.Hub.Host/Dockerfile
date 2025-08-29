FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["apps/hub/src/Qorpe.Hub.Host/Qorpe.Hub.Host.csproj", "apps/hub/src/Qorpe.Hub.Host/"]
RUN dotnet restore "apps/hub/src/Qorpe.Hub.Host/Qorpe.Hub.Host.csproj"
COPY . .
WORKDIR "/src/apps/hub/src/Qorpe.Hub.Host"
RUN dotnet build "./Qorpe.Hub.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Qorpe.Hub.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Qorpe.Hub.Host.dll"]
