#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Src/CoreShopUms/CoreShopUms.csproj", "Src/CoreShopUms/"]
COPY ["Src/EventBus/EventBus.csproj", "Src/EventBus/"]
RUN dotnet restore "Src/CoreShopUms/CoreShopUms.csproj"
COPY . .
WORKDIR "/src/Src/CoreShopUms"
RUN dotnet build "CoreShopUms.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreShopUms.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreShopUms.dll"]