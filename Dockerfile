# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/AgroSolutions.Analytics.Service.Api/AgroSolutions.Analytics.Service.Api.csproj", "src/AgroSolutions.Analytics.Service.Api/"]
COPY ["src/AgroSolutions.Analytics.Service.Application/AgroSolutions.Analytics.Service.Application.csproj", "src/AgroSolutions.Analytics.Service.Application/"]
COPY ["src/AgroSolutions.Analytics.Service.Domain/AgroSolutions.Analytics.Service.Domain.csproj", "src/AgroSolutions.Analytics.Service.Domain/"]
COPY ["src/AgroSolutions.Analytics.Service.Infra/AgroSolutions.Analytics.Service.Infra.csproj", "src/AgroSolutions.Analytics.Service.Infra/"]

# Restaurar dependências
RUN dotnet restore "src/AgroSolutions.Analytics.Service.Api/AgroSolutions.Analytics.Service.Api.csproj"

# Copiar tudo
COPY . .

# Build
WORKDIR "/src/src/AgroSolutions.Analytics.Service.Api"
RUN dotnet build "AgroSolutions.Analytics.Service.Api.csproj" -c Release -o /app/build

# Estágio de publicação
FROM build AS publish
RUN dotnet publish "AgroSolutions.Analytics.Service.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install -y wget && rm -rf /var/lib/apt/lists/*
ENTRYPOINT ["dotnet", "AgroSolutions.Analytics.Service.Api.dll"]
