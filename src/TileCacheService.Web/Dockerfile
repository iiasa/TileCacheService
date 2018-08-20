FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 6349
EXPOSE 44352

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/TileCacheService.Web/TileCacheService.Web.csproj", "src/TileCacheService.Web/"]
COPY ["src/TileCacheService.Processing/TileCacheService.Processing.csproj", "src/TileCacheService.Processing/"]
COPY ["src/TileCacheService.Shared/TileCacheService.Shared.csproj", "src/TileCacheService.Shared/"]
COPY ["src/TileCacheService.Data/TileCacheService.Data.csproj", "src/TileCacheService.Data/"]
COPY ["NuGet.Config", "NuGet.Config"]
RUN dotnet restore "src/TileCacheService.Web/TileCacheService.Web.csproj"
COPY . .
WORKDIR "/src/src/TileCacheService.Web"
RUN dotnet build "TileCacheService.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TileCacheService.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TileCacheService.Web.dll"]