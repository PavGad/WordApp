#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WordApp.Api/WordApp.Api.csproj", "WordApp.Api/"]
COPY ["WordApp.Domain/WordApp.Domain.csproj", "WordApp.Domain/"]
COPY ["WordApp.Persistence/WordApp.Persistence.csproj", "WordApp.Persistence/"]
COPY ["WordApp.Shared/WordApp.Shared.csproj", "WordApp.Shared/"]
RUN dotnet restore "WordApp.Api/WordApp.Api.csproj"
COPY . .
WORKDIR "/src/WordApp.Api"
RUN dotnet build "WordApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WordApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WordApp.Api.dll"]