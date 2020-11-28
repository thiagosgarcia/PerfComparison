#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["net21/net21.csproj", "net21/"]
RUN dotnet restore "net21/net21.csproj"
COPY . .
WORKDIR "/src/net21"
RUN dotnet publish "net21.csproj" -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS publish
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app .
ENTRYPOINT ["dotnet", "net21.dll"]