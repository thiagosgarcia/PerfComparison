#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["net21/net21.csproj", "net21/"]
RUN dotnet restore "net21/net21.csproj"
COPY . .
WORKDIR "/src/net21"
RUN dotnet build "net21.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "net21.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "net21.dll"]