#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["net31/net31.csproj", "net31/"]
RUN dotnet restore "net31/net31.csproj"
COPY . .
WORKDIR "/src/net31"
RUN dotnet publish "net31.csproj" -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS publish
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app .
ENTRYPOINT ["dotnet", "net31.dll"]