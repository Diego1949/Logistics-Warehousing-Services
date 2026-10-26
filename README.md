# SGCAE – Sistema de Gestión y Control de Acceso de Empleados

## 1. Descripción

SGCAE es un backend con API REST para una empresa de logística que hoy registra en planillas físicas las entradas y salidas de su personal. El sistema permite:

- Gestionar **empleados** (alta, consulta, actualización y desactivación).
- Asignar y administrar **credenciales** de acceso (bloqueo y revocación).
- Administrar **puntos de acceso** (Recepción, Parqueadero, Bodega, Área Restringida, etc.).
- **Registrar movimientos** de entrada/salida, autorizados o rechazados con su motivo.
- Consultar el **historial** de accesos con filtros y paginación.

Stack: .NET 8, ASP.NET Core Web API (controllers), Entity Framework Core 8, SQL Server y Swagger.

---

## 2. Arquitectura

La solución sigue **Clean Architecture** con cuatro proyectos. Las dependencias siempre apuntan hacia el dominio:

```
            ┌──────────────────────────────┐
            │          Sgcae.Api           │  Controllers, manejo global de errores, Swagger
            └──────┬───────────────┬───────┘
                   │               │
                   ▼               ▼
┌──────────────────────────┐  ┌──────────────────────────┐
│   Sgcae.Infrastructure   │─▶│    Sgcae.Application     │  Commands, Queries, Handlers, DTOs
│ EF Core, repositorios,   │  └────────────┬─────────────┘
│ migraciones, consultas   │               │
└────────────┬─────────────┘               │
             │                             ▼
             │              ┌──────────────────────────┐
             └─────────────▶│       Sgcae.Domain       │  Entidades, value objects, reglas
                            └──────────────────────────┘
```

| Proyecto | Depende de | Responsabilidad |
|---|---|---|
| `Sgcae.Domain` | (nada) | Entidades, value objects, enums, invariantes, errores de dominio, `Result`, interfaces de repositorio. |
| `Sgcae.Application` | Domain | Casos de uso (CQRS), DTOs, `PagedResult<T>`, abstracciones (`IUnitOfWork`, consultas de lectura). |
| `Sgcae.Infrastructure` | Application, Domain | `SgcaeDbContext`, configuraciones Fluent API, repositorios, consultas de lectura, migraciones, seed. |
| `Sgcae.Api` | Application, Infrastructure | Controllers delgados, `ProblemDetails`, manejo global de excepciones, Swagger, `Program.cs`. |

### Dónde está DDD (`Sgcae.Domain`)

- **Entidades / agregados**: `Employee`, `Credential`, `AccessPoint`, `AccessLog`.
- **Value object**: `Email` (valida formato y normaliza a minúsculas).
- **Encapsulamiento**: constructores privados, métodos fábrica (`Employee.Create`, `AccessPoint.Create`, `AccessLog.Authorized/Rejected`), setters privados y colección encapsulada (`Employee.Credentials` es de solo lectura; la credencial se crea mediante `Employee.AssignCredential`).
- **Comportamiento con reglas**: `Deactivate`, `Block`, `Revoke`, `CanBeUsed`, `Enable`, `Disable`.
- **Patrón Result**: los métodos de dominio devuelven `Result` / `Result<T>` con un `Error` tipado (`Validation`, `NotFound`, `Conflict`) en lugar de lanzar excepciones.

### Dónde está CQRS (`Sgcae.Application`)

- Implementado a mano, sin MediatR: interfaces `ICommand`, `ICommand<T>`, `IQuery<T>`, `ICommandHandler<>`, `ICommandHandler<,>` e `IQueryHandler<,>` en `Abstractions/Messaging`.
- Los handlers se registran automáticamente por reflexión en `DependencyInjection.AddApplication()`.
- **Lado de escritura (commands)**: usan los repositorios del dominio y `IUnitOfWork`.
- **Lado de lectura (queries)**: usan interfaces `I*Queries` implementadas en Infrastructure con `AsNoTracking()` y proyección directa a DTO.
- Los controllers solo construyen el command/query, lo despachan al handler inyectado y traducen el `Result` a HTTP.

---

## 3. Estructura de carpetas

```
.
├── Sgcae.sln
├── Directory.Build.props
├── requests.http
├── README.md
└── src
    ├── Sgcae.Domain
    │   ├── Common            (Entity, Result, Error, Guard)
    │   ├── Employees         (Employee, Email, EmployeeStatus, EmployeeErrors, IEmployeeRepository)
    │   ├── Credentials       (Credential, CredentialStatus, CredentialErrors, ICredentialRepository)
    │   ├── AccessPoints      (AccessPoint, AccessPointErrors, IAccessPointRepository)
    │   └── AccessLogs        (AccessLog, MovementType, AccessResult, RejectionReason, IAccessLogRepository)
    ├── Sgcae.Application
    │   ├── Abstractions      (Messaging: ICommand, IQuery, handlers; Data: IUnitOfWork)
    │   ├── Common            (PagedResult, Pagination)
    │   ├── Employees         (Commands, Queries, EmployeeDto, IEmployeeQueries)
    │   ├── Credentials       (Commands, Queries, CredentialDto, ICredentialQueries)
    │   ├── AccessPoints      (Commands, Queries, AccessPointDto, IAccessPointQueries)
    │   ├── AccessLogs        (Commands, Queries, AccessLogDto, IAccessLogQueries)
    │   └── DependencyInjection.cs
    ├── Sgcae.Infrastructure
    │   ├── Persistence       (SgcaeDbContext, Configurations, Converters, Migrations, DatabaseInitializer)
    │   ├── Repositories      (implementaciones de los repositorios del dominio)
    │   ├── Queries           (consultas de lectura con AsNoTracking + proyección)
    │   └── DependencyInjection.cs
    └── Sgcae.Api
        ├── Controllers       (ApiController base + 4 controllers)
        ├── Contracts         (UpdateEmployeeRequest, RegisterAccessRequest)
        ├── ErrorHandling     (GlobalExceptionHandler)
        ├── Properties        (launchSettings.json)
        ├── appsettings.json
        └── Program.cs
```

---

## 4. Requisitos previos

| Herramienta | Uso | Cómo verificar |
|---|---|---|
| **.NET 8 SDK** | Compilar y ejecutar | `dotnet --list-sdks` (debe aparecer una versión `8.0.x`) |
| **SQL Server LocalDB** (incluido con Visual Studio) **o** SQL Server (local, Express o Docker) | Base de datos | `sqllocaldb info` (debe listar `MSSQLLocalDB`) |
| **git** | Clonar el repositorio | `git --version` |
| **dotnet-ef** (opcional) | Aplicar migraciones manualmente | `dotnet ef --version` · instalar con `dotnet tool install --global dotnet-ef --version 8.*` |

---

## 5. Paso a paso de ejecución

### 5.1 Clonar

```bash
git clone <url-del-repositorio>
cd Logistics-Warehousing-Services
```

### 5.2 Configurar la connection string

Por defecto `src/Sgcae.Api/appsettings.json` apunta a LocalDB y no requiere cambios:

```json
"ConnectionStrings": {
  "SgcaeDb": "Server=(localdb)\\MSSQLLocalDB;Database=SgcaeDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**Alternativa A – SQL Server en Docker:**

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_strong_Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

y cambie la connection string por:

```
Server=localhost,1433;Database=SgcaeDb;User Id=sa;Password=Your_strong_Passw0rd;TrustServerCertificate=True;
```

**Alternativa B – SQL Server con usuario y contraseña** (instancia existente):

```
Server=MI_SERVIDOR;Database=SgcaeDb;User Id=mi_usuario;Password=mi_password;TrustServerCertificate=True;
```

También puede sobrescribirla sin editar archivos mediante una variable de entorno:

```bash
# PowerShell
$env:ConnectionStrings__SgcaeDb = "Server=localhost,1433;Database=SgcaeDb;User Id=sa;Password=Your_strong_Passw0rd;TrustServerCertificate=True;"
# bash
export ConnectionStrings__SgcaeDb="Server=localhost,1433;Database=SgcaeDb;User Id=sa;Password=Your_strong_Passw0rd;TrustServerCertificate=True;"
```

### 5.3 Restaurar y compilar

```bash
dotnet restore
dotnet build
```

### 5.4 Ejecutar

```bash
dotnet run --project src/Sgcae.Api
```

La API queda escuchando en **http://localhost:5080**.

### 5.5 Swagger

Abra **http://localhost:5080/swagger** para ver y probar todos los endpoints. Como alternativa, el archivo `requests.http` de la raíz contiene un ejemplo de cada endpoint en orden lógico (se ejecuta con Visual Studio 2022 o con la extensión *REST Client* de VS Code; los IDs se encadenan automáticamente entre peticiones).

### 5.6 Migraciones

- **Automáticas**: al iniciar, la API ejecuta `Database.Migrate()` (crea la base `SgcaeDb` si no existe y aplica las migraciones pendientes) y siembra los puntos de acceso iniciales: *Reception*, *Parking*, *Warehouse* y *Restricted Area*. En consola verá `Applying migration '..._InitialCreate'` la primera vez.
- **Manual (alternativa)**, con `dotnet-ef` instalado:

```bash
dotnet ef database update --project src/Sgcae.Infrastructure --startup-project src/Sgcae.Api
```

Para empezar de cero: `dotnet ef database drop -f --project src/Sgcae.Infrastructure --startup-project src/Sgcae.Api`.

---

## 6. Endpoints

| Req. | Método | Ruta | Descripción | Respuestas |
|---|---|---|---|---|
| RF-01 | POST | `/api/employees` | Crear empleado | 201, 400, 409 |
| RF-01 | GET | `/api/employees/{id}` | Obtener empleado | 200, 404 |
| RF-01 | GET | `/api/employees?page=1&pageSize=20` | Listar empleados paginado | 200, 400 |
| RF-01 | PUT | `/api/employees/{id}` | Actualizar empleado | 200, 400, 404, 409 |
| RF-01 | PATCH | `/api/employees/{id}/deactivate` | Desactivar empleado | 204, 404, 409 |
| RF-02 | POST | `/api/credentials` | Asignar credencial a un empleado | 201, 400, 404, 409 |
| RF-02 | GET | `/api/credentials/{id}` | Obtener credencial | 200, 404 |
| RF-02 | GET | `/api/credentials/employee/{employeeId}` | Credenciales de un empleado | 200, 404 |
| RF-02 | PATCH | `/api/credentials/{id}/block` | Bloquear credencial | 204, 404, 409 |
| RF-02 | PATCH | `/api/credentials/{id}/revoke` | Revocar credencial | 204, 404, 409 |
| RF-03 | POST | `/api/access-logs` | Registrar entrada/salida | 200 (autorizado o rechazado), 400 |
| RF-04 | GET | `/api/access-logs?employeeId=&from=&to=&accessPointId=&result=&page=&pageSize=` | Historial filtrado y paginado | 200, 400 |
| RF-06 | POST | `/api/access-points` | Crear punto de acceso | 201, 400, 409 |
| RF-06 | GET | `/api/access-points` | Listar puntos de acceso | 200 |
| RF-06 | GET | `/api/access-points/{id}` | Obtener punto de acceso | 200, 404 |
| RF-06 | PATCH | `/api/access-points/{id}/enable` | Habilitar punto de acceso | 204, 404, 409 |
| RF-06 | PATCH | `/api/access-points/{id}/disable` | Deshabilitar punto de acceso | 204, 404, 409 |

Notas:
- Los errores se devuelven como **ProblemDetails** (`application/problem+json`) con un campo adicional `code` (por ejemplo `Employee.DuplicateCode`).
- Los enums viajan como texto: `movementType` = `Entry` | `Exit`; `result` = `Authorized` | `Rejected`.
- `pageSize` admite de 1 a 100 (por defecto 20). Las fechas `from`/`to` se interpretan en UTC (ej. `2026-10-26T00:00:00Z`).

---

## 7. Flujo de prueba sugerido

Todos los pasos pueden hacerse desde Swagger o con `requests.http`.

1. **Crear empleado** – `POST /api/employees`
   ```json
   { "employeeCode": "EMP-001", "firstName": "Laura", "lastName": "Gomez",
     "email": "laura.gomez@logistics.com", "jobTitle": "Forklift Operator", "department": "Warehouse" }
   ```
   → 201 con header `Location`. Guarde el `id`.
2. **Asignar credencial** – `POST /api/credentials`
   ```json
   { "employeeId": "<id-empleado>", "code": "CRED-001" }
   ```
   → 201. Guarde el `id` de la credencial.
3. **Crear punto de acceso** – `POST /api/access-points`
   ```json
   { "name": "Loading Dock", "description": "Truck loading dock" }
   ```
   → 201. Guarde el `id` (o use uno sembrado obtenido con `GET /api/access-points`).
4. **Registrar entrada autorizada** – `POST /api/access-logs`
   ```json
   { "credentialCode": "CRED-001", "accessPointId": "<id-punto>", "movementType": "Entry" }
   ```
   → 200 con `"result": "Authorized"` y `"rejectionReason": null`.
5. **Bloquear la credencial** – `PATCH /api/credentials/<id-credencial>/block` → 204.
6. **Registrar intento rechazado** – repetir el paso 4 → 200 con `"result": "Rejected"` y `"rejectionReason": "CredentialBlocked"`.
7. **Consultar historial con filtros** –
   `GET /api/access-logs?employeeId=<id-empleado>&result=Rejected&page=1&pageSize=10`
   → `PagedResult` con `items`, `page`, `pageSize`, `totalCount`, `totalPages`, ordenado del más reciente al más antiguo.

---

## 8. Reglas de negocio implementadas

**Empleados**
- `EmployeeCode`, nombres, correo, cargo y departamento son obligatorios y tienen longitud máxima.
- El correo es un value object con validación de formato; se guarda en minúsculas.
- `EmployeeCode` y `Email` son únicos (409 si se repiten).
- Un empleado nace `Active`; no puede desactivarse dos veces (409).

**Credenciales**
- Solo se asigna credencial a un empleado **activo** (409 en caso contrario).
- `Code` es único (409).
- Estados: `Active`, `Blocked`, `Revoked`. Solo una credencial `Active` puede usarse (`CanBeUsed`).
- Una credencial revocada **no puede bloquearse ni reactivarse**: `Revoked` es un estado final. Tampoco puede bloquearse o revocarse dos veces.

**Puntos de acceso**
- `Name` obligatorio y único. Se crean habilitados.
- No se puede habilitar uno ya habilitado ni deshabilitar uno ya deshabilitado (409).

**Registro de accesos (RF-03)**
- Se valida en este orden y se detiene en el primer fallo:
  1. El punto de acceso existe → si no, `AccessPointNotFound`.
  2. El punto de acceso está habilitado → si no, `AccessPointDisabled`.
  3. La credencial existe → si no, `CredentialNotFound`.
  4. La credencial puede usarse → si no, `CredentialBlocked` o `CredentialRevoked`.
  5. El empleado está activo → si no, `EmployeeInactive`.
- **Siempre se registra el movimiento**, autorizado o rechazado, y se responde **200** con el resultado y el motivo.
- No se valida la secuencia entrada/salida.
- La fecha `OccurredAt` se guarda en UTC.
- `AccessLog.AccessPointId` no tiene clave foránea para poder auditar intentos sobre puntos inexistentes.

**Historial (RF-04)**
- Filtros opcionales: `employeeId`, `from`, `to`, `accessPointId`, `result`.
- `page` ≥ 1, `pageSize` entre 1 y 100, `from` ≤ `to` (400 si no se cumple).
- Ordenado por `OccurredAt` descendente; consulta con `AsNoTracking` y proyección a DTO.

**Persistencia**
- Índices únicos: `Employees.EmployeeCode`, `Employees.Email`, `Credentials.Code`, `AccessPoints.Name`.
- Índices en `AccessLogs`: `OccurredAt`, `EmployeeId`, `AccessPointId`.
- Enums guardados como texto.

---

## 9. Solución de problemas comunes

**El puerto 5080 está ocupado** (`Failed to bind to address http://127.0.0.1:5080: address already in use`)
- Ejecute en otro puerto: `dotnet run --project src/Sgcae.Api --urls http://localhost:5090`
- O identifique el proceso: `netstat -ano | findstr :5080` (Windows) / `lsof -i :5080` (Linux/macOS) y ciérrelo.
- Si cambia el puerto, actualice `@baseUrl` en `requests.http`.

**Error de conexión a SQL Server** (`A network-related or instance-specific error occurred...`)
- Verifique que la instancia esté en ejecución: `sqllocaldb info MSSQLLocalDB` y, si está detenida, `sqllocaldb start MSSQLLocalDB`.
- Con Docker, compruebe que el contenedor esté arriba (`docker ps`) y que el puerto 1433 esté publicado.
- Revise usuario, contraseña y nombre del servidor en la connection string. Mantenga `TrustServerCertificate=True` en entornos locales.
- Si el error es `Login failed for user`, el usuario no tiene permisos para crear la base de datos; use uno con rol `dbcreator` o cree la base `SgcaeDb` manualmente.

**LocalDB no está instalado** (`sqllocaldb` no se reconoce)
- LocalDB solo existe en Windows. Instálelo desde el instalador de Visual Studio (componente *SQL Server Express LocalDB*) o con el instalador de SQL Server Express eligiendo *LocalDB*.
- En Linux/macOS, o si prefiere no instalarlo, use SQL Server en Docker (sección 5.2, alternativa A).

**`dotnet` no encuentra el SDK 8**
- Instale el .NET 8 SDK desde https://dotnet.microsoft.com/download/dotnet/8.0 y verifique con `dotnet --list-sdks`.
