#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JewelryAuctionWebAPI/JewelryAuctionWebAPI.csproj", "JewelryAuctionWebAPI/"]
COPY ["JewelryAuctionBusiness/JewelryAuctionBusiness.csproj", "JewelryAuctionBusiness/"]
COPY ["JewelryAuctionData/JewelryAuctionData.csproj", "JewelryAuctionData/"]
RUN dotnet restore "./JewelryAuctionWebAPI/JewelryAuctionWebAPI.csproj"
COPY . .
WORKDIR "/src/JewelryAuctionWebAPI"
RUN dotnet build "./JewelryAuctionWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JewelryAuctionWebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JewelryAuctionWebAPI.dll"] 