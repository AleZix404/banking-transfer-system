FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR bankingTransferSystem
EXPOSE 80
EXPOSE 5000

#Copiar archivos de proyecto
COPY ./*.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

#Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /bankingTransferSystem
COPY --from=build /bankingTransferSystem/out .
ENTRYPOINT ["dotnet","banking-transfer-system.dll"]