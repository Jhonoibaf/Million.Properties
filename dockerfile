# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo de solución y los csproj de cada proyecto
COPY *.sln ./
COPY Million.API/*.csproj ./Million.API/
COPY Million.Properties.Application/*.csproj ./Million.Properties.Application/
COPY Million.Properties.Domain/*.csproj ./Million.Properties.Domain/
COPY Million.Properties.Infrastructure/*.csproj ./Million.Properties.Infrastructure/
COPY Million.Properties.Application.UnitTest/*.csproj ./Million.Properties.Application.UnitTest/
COPY Million.Properties.IntegrationTest/*.csproj ./Million.Properties.IntegrationTest/
COPY Million.Properties.Data.Test/*.csproj ./Million.Properties.Data.Test/
COPY Million.Commons/*.csproj ./Million.Commons/
# Restaurar dependencias
RUN dotnet restore

# Copiar el resto del código
COPY . .

# Publicar la API principal
WORKDIR /src/Million.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false
# Nota: UseAppHost=false evita generar un exe para Windows, útil en Linux
RUN ls /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar el resultado de la publicación
COPY --from=build /app/publish ./

# Exponer puerto
EXPOSE 5000

# Variables de entorno
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENV MongoSettings__ConnectionString=mongodb://mongo:27017
ENV MongoSettings__DatabaseName=million

# Comando para correr la app
ENTRYPOINT ["dotnet", "Million.Properties.API.dll"]
