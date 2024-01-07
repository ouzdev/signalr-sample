FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SiganlR.Server/SiganlR.Server.csproj", "SiganlR.Server/"]
RUN dotnet restore "SiganlR.Server/SiganlR.Server.csproj"
COPY . .
WORKDIR "/src/SiganlR.Server"
RUN dotnet build "SiganlR.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SiganlR.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SiganlR.Server.dll"]
