FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DynamicImageAPI/DynamicImageAPI.csproj", "DynamicImageAPI/"]

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DynamicImageAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DynamicImageAPI.dll"]
