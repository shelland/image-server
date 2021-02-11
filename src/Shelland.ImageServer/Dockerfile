#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Shelland.ImageServer/Shelland.ImageServer.csproj", "Shelland.ImageServer/"]
COPY ["Shelland.ImageServer.Core/Shelland.ImageServer.Core.csproj", "Shelland.ImageServer.Core/"]
COPY ["Shelland.ImageServer.DataAccess/Shelland.ImageServer.DataAccess.csproj", "Shelland.ImageServer.DataAccess/"]
COPY ["Shelland.ImageServer.AppServices/Shelland.ImageServer.AppServices.csproj", "Shelland.ImageServer.AppServices/"]
RUN dotnet restore "Shelland.ImageServer/Shelland.ImageServer.csproj"
COPY . .
WORKDIR "/src/Shelland.ImageServer"
RUN dotnet build "Shelland.ImageServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shelland.ImageServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shelland.ImageServer.dll"]