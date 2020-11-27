#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["net31/net31.csproj", "net31/"]
RUN dotnet restore "net31/net31.csproj"
COPY . .
WORKDIR "/src/net31"
RUN dotnet build "net31.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "net31.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "net31.dll"]