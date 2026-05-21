# Etapa 1: Compilación con el SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
# (Ajustá los nombres de las carpetas/proyectos según tu estructura real)
COPY ["Licencias.Api/Licencias.Api.csproj", "Licencias.Api/"]
COPY ["Licencias.Domain/Licencias.Domain.csproj", "Licencias.Domain/"]
COPY ["Licencias.Infrastructure/Licencias.Infrastructure.csproj", "Licencias.Infrastructure/"]
COPY ["Licencias.Application/Licencias.Application.csproj", "Licencias.Application/"]

RUN dotnet restore "Licencias.Api/Licencias.Api.csproj"

# Copiar el resto del código y compilar
COPY . .
WORKDIR "/src/Licencias.Api"
RUN dotnet publish "Licencias.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Imagen final liviana para correr la API
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Licencias.Api.dll"] 