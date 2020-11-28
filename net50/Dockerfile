#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["net50/net50.csproj", "net50/"]
RUN dotnet restore "net50/net50.csproj"
COPY . .
WORKDIR "/src/net50"
RUN dotnet publish "net50.csproj" -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS publish
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app .
ENTRYPOINT ["dotnet", "net50.dll"]