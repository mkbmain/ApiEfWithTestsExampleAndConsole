FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SimpleEfApi/SimpleEfApi.csproj", "SimpleEfApi/"]
RUN dotnet restore "SimpleEfApi/SimpleEfApi.csproj"
COPY . .
WORKDIR "/src/SimpleEfApi"
RUN dotnet build "SimpleEfApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleEfApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleEfApi.dll"]
