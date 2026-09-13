FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["PersonasAPI.csproj", "."]
RUN dotnet restore "./PersonasAPI.csproj"
COPY . .
RUN dotnet build "./PersonasAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./PersonasAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonasAPI.dll"]