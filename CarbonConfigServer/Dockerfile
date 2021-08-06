FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CarbonConfigServer/CarbonConfigServer.csproj", "CarbonConfigServer/"]
RUN dotnet restore "CarbonConfigServer/CarbonConfigServer.csproj"
COPY . .
WORKDIR "/src/CarbonConfigServer"
RUN dotnet build "CarbonConfigServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarbonConfigServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarbonConfigServer.dll"]
