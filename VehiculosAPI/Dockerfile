FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["VehiculosAPI.csproj", "."]
RUN dotnet restore "./VehiculosAPI.csproj"
COPY . .
RUN dotnet build "./VehiculosAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./VehiculosAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VehiculosAPI.dll"]