FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SportsStore.csproj", "."]
RUN dotnet restore "SportsStore.csproj"
COPY . .
RUN dotnet build "SportsStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SportsStore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SportsStore.dll"]