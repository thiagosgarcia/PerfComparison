#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Tester/Tester.csproj", "Tester/"]
RUN dotnet restore "Tester/Tester.csproj"
COPY . .
WORKDIR "/src/Tester"
RUN dotnet publish "Tester.csproj" -c Release -o /app/publish  --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS publish
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Tester.dll"]
CMD ["net21", "net31", "net50", "-c", "20", "-n", "100", "-d", "1"]