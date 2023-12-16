#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/NotificationService.Api/NotificationService.Api.csproj", "src/NotificationService.Api/"]
COPY ["src/NotificationService.Business/NotificationService.Business.csproj", "src/NotificationService.Business/"]
COPY ["src/NotificationService.Repository/NotificationService.Repository.csproj", "src/NotificationService.Repository/"]
COPY ["src/NotificationService.Shared/NotificationService.Shared.csproj", "src/NotificationService.Shared/"]
RUN dotnet restore "src/NotificationService.Api/NotificationService.Api.csproj"
COPY . .
WORKDIR "/src/src/NotificationService.Api"
RUN dotnet build "NotificationService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Api.dll"]