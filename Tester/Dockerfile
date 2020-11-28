#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Tester/Tester.csproj", "Tester/"]
RUN dotnet restore "Tester/Tester.csproj"
COPY . .
WORKDIR "/src/Tester"
#RUN dotnet build "Tester.csproj" -c Release -o /app/build --no-restore -v n

FROM build AS publish
RUN dotnet publish "Tester.csproj" -c Release -o /app/publish -v n  --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tester.dll"]
CMD ["net21", "net31", "net50", "-c", "20", "-n", "100", "-d", "0")]