# 🏡 Million.Properties

Million.Properties es una **API desarrollada en .NET 8** para gestionar propiedades inmobiliarias, siguiendo principios de **Clean Architecture** y el patrón **CQRS** mediante **MediatR**.

La capa de infraestructura utiliza **Entity Framework Core con SQL Server** para la persistencia, integrando repositorios concretos dentro de `Infrastructure`.

---

## 🚀 Funcionalidades

La API permite:

- Consultar todas las propiedades con filtros.
- Consultar una propiedad por ID.
- Crear nuevas propiedades.
- Asociar imágenes a una propiedad.
- Recuperar todas las imágenes asociadas a una propiedad.

---

## 🛠️ Tecnologías Usadas

### Backend
- **.NET 8**
- **Entity Framework Core (SQL Server)**
- **MediatR** (CQRS)
- **AutoMapper**
- **FluentValidation** (si aplica)
- **Swagger / Swashbuckle**

### Infraestructura
- **SQL Server 2022** en contenedor Docker
- **EF Core Migrations**
- **Docker Compose**

### Testing
- **NUnit**
- **Moq**
- **Testcontainers** (si aplica para integración)

---

## 📐 Arquitectura (Clean Architecture)
src/
├── Million.Properties.API                 → Capa de Presentación (Controllers, Swagger)
├── Million.Properties.Application         → Lógica de negocio (CQRS)
├── Million.Properties.Domain              → Entidades del dominio
├── Million.Properties.Infrastructure      → Persistencia (EF Core, SQL Server, Repositorios)

tests/
├── Million.Properties.Application.UnitTest
├── Million.Properties.IntegrationTest

### API (Presentation)
- Configuración de servicios
- Inyección de dependencias
- Controladores REST
- Documentación Swagger

### Application
- Commands y Queries con MediatR
- DTOs
- Interfaces de repositorio
- Mappers con AutoMapper

### Domain
- Entidades del dominio (Property, PropertyImage, Owner, etc.)

### Infrastructure
- SQL Server DbContext
- EntityTypeConfigurations
- Repositorios concretos
- Migraciones EF Core

---

## ▶️ Ejecución del Proyecto

### 1. Clonar el repositorio

```bash
git clone https://github.com/Jhonoibaf/Million.Properties
cd Million.Properties
```
### 🗄️ 2. Crear la base de datos

En SQL Server crea una base de datos vacía llamada:
---

## ⚙️ 3. Configurar la cadena de conexión

Edita el archivo: AppSettins o SecretsManagger

Agrega o actualiza:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MillionPropertiesDb;User Id=sa;Password=TuPasswordSegura;TrustServerCertificate=True;"
  }
}
```
---
## ▶️ Migraciones con EF Core


```bash
dotnet tool install --global dotnet-ef
cd src/Million.Properties.Infrastructure
dotnet ef migrations add InitialCreate -s ../Million.Properties.API
dotnet ef database update -s ../Million.Properties.API
```
---

## ▶️ Ejecutar la API

```bash
cd src/Million.Properties.API
dotnet run
```
### La API estará disponible en:

- https://localhost:7206/swagger/index.html
- http://localhost:5000/swagger


## Endpoints

### Propiedades

- GET /api/v1/Property/GetAllProperty
- GET /api/v1/Property/GetProperty/{id}
- POST /api/v1/Property/CreateProperty
- PUT /api/v1/Property/UpdateProperty
- PUT /api/v1/Property/UpdatePropertyPriceById/{Id}/{price}

### Imágenes

- POST /api/v1/Images/CreateImage