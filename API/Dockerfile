FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Warehouse.API/Warehouse.API.csproj", "Warehouse.API/"]
RUN dotnet restore "Warehouse.API/Warehouse.API.csproj"
COPY . .
WORKDIR "/src/Warehouse.API"
RUN dotnet build "Warehouse.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Warehouse.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Warehouse.API.dll"]
