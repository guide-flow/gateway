FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Gateway/*.csproj Gateway/
COPY *.sln ./
RUN dotnet restore 
COPY . .
WORKDIR "/src/Gateway"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false --no-restore
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT [ "dotnet", "Gateway.dll" ]