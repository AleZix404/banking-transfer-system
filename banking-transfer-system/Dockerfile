# Usa la imagen oficial de .NET SDK con soporte multi-arquitectura
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copia los archivos del proyecto y restaura las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copia el resto de los archivos y construye la aplicacion
COPY . ./
RUN dotnet publish -c Release -o out

# Usa la imagen runtime de .NET con soporte multi-arquitectura
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expone el puerto 80 para el trafico HTTP
EXPOSE 80

# Configura la entrada para ejecutar la aplicacion
ENTRYPOINT ["dotnet", "banking-transfer-system.dll"]
