# banking-transfer-system
API para gestionar transferencias bancarias entre cuentas. Permite transferencias, validación de saldo, y consulta de estado de cuenta e historial de transacciones.
### Requisitos previos:
- .NET Core SDK 8.0 o superior
- PostgreSQL 17
- Git
- Postman (opcional, para probar la API)
- Docker y Docker Compose
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
### Compilar y ejecutar la aplicación:
1. Navega al directorio raíz del proyecto.
2. Ejecuta el siguiente comando para restaurar dependencias y compilar:
```bash
dotnet restore
dotnet build
