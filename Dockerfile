# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY . .

RUN dotnet restore "./Domain/Domain.csproj" --disable-parallel
RUN dotnet restore "./Infrastructure/Infrastructure.csproj" --disable-parallel
RUN dotnet restore "./API/API.csproj" --disable-parallel

RUN dotnet publish "./API/API.csproj" -o /app --no-restore
#Serve Stage

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal 
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80

ENTRYPOINT ["dotnet", "API.dll", "--environment","Development"]