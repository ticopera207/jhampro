# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY jhampro/Jham.csproj jhampro/
WORKDIR /src/jhampro
RUN dotnet restore

# Copiar el resto del código y compilar
COPY jhampro/. .
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar archivos publicados
COPY --from=build-env /app/publish .

# Soporte para globalización (si usas formatos regionales como fechas/lenguajes)
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    locales && \
    rm -rf /var/lib/apt/lists/* && \
    locale-gen es_PE.UTF-8
ENV LANG es_PE.UTF-8
ENV LANGUAGE es_PE:es
ENV LC_ALL es_PE.UTF-8

# Configuraciones Render
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080

# Cambiar esto si tu archivo real es distinto
ENTRYPOINT ["dotnet", "Jham.dll"]
