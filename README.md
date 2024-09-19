# banking-transfer-system
API para gestionar transferencias bancarias entre cuentas. Permite transferencias, validación de saldo, y consulta de estado de cuenta e historial de transacciones. Desarrollada en .NET con Entity Framework Core y PostgreSQL.
API para gestionar transferencias bancarias entre cuentas. Permite transferencias, validación de saldo, y consulta de estado de cuenta e historial de transacciones.
### Requisitos previos:
- .NET Core SDK 8.0 o superior
- PostgreSQL 17
- Git
- Postman (opcional, para probar la API)
### Clonar el repositorio:
```bash
git clone https://github.com/tu-usuario/banking-transfer-system.git
cd banking-transfer-system
```
### Configuración de la base de datos:
1. Configura PostgreSQL en tu máquina y crea una base de datos llamada `banking_transactions_db`.
2. Ejecuta el script: bank_transfer_schema_v1.sql.
3. En el proyecto banking-transfer-system de .NET, actualiza la cadena de conexión en el archivo `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=banking_transactions_db;Username=soporte;Password=soporte"
}
```
### Configuración JWT (Json Web Token):
La aplicación utiliza JWT para autenticación. Usa las claves JWT estan configuradas correctamente en el archivo appsettings.json:
```json
"Jwt": {
  "Key": "clave-secreta-para-el-jwt",
  "Issuer": "http://localhost",
  "Audience": "http://localhost"
}
```
### Configuración del logging con Serilog
Para asegurar un correcto manejo de logs, la aplicación utiliza Serilog. Los logs se pueden configurar para que se guarden en la consola o en archivos. En el archivo Program.cs ya está configurado Serilog de la siguiente manera:
```
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Otras configuraciones...

app.UseSerilogRequestLogging();
```
### Compilar y ejecutar la aplicación:
1. Navega al directorio raíz del proyecto.
2. Ejecuta el siguiente comando para restaurar dependencias y compilar:
```bash
dotnet restore
dotnet build
```
### Probar la API:
Para probar la API puedes usar Postman o cualquier cliente HTTP. Aquí algunos ejemplos de cómo puedes probar la API:
Registro de usuario:
Endpoint: POST /api/Auth/register
```json
Payload:
{
  "Username": "juan perez",
  "Password": "123456"
}
```
### Autenticación de usuario (genera un JWT):
Endpoint: POST /api/Auth/login
Payload:
```json
Payload:
{
  "Username": "juan perez",
  "Password": "123456"
}
```
Respuesta:
```json
{
  "token": "jwt-token-generado"
}
```
### Crear transferencia:
Endpoint: POST /api/Account/createTransfer
Headers:
Authorization: Bearer jwt-token
```json
Payload:
{
  "SourceAccountNumber": "12345678",
  "DestinationAccountNumber": "87654321",
  "Amount": 100.00
}
```
### Consultar historial de cuenta:
Endpoint: GET /api/Account/history/{accountNumber}
Headers:
Authorization: Bearer jwt-token
### Consultar estado de cuenta:
Endpoint: GET /api/Account/status/{accountNumber}
Headers:
Authorization: Bearer jwt-token
