FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleAppExample/ConsoleAppExample.csproj", "ConsoleAppExample/"]
RUN dotnet restore "ConsoleAppExample/ConsoleAppExample.csproj"
COPY . .
WORKDIR "/src/ConsoleAppExample"
RUN dotnet build "ConsoleAppExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleAppExample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsoleAppExample.dll"]
