# Usando a imagem base do SDK .NET para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar os arquivos do projeto para o container
COPY *.csproj .
RUN dotnet restore

# Copiar todo o código para o container e realizar o build
COPY . .
RUN dotnet publish -c Release -o /publish

# Usando a imagem base do Runtime .NET para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar os binários do build para o container final
COPY --from=build /publish .

# Porta padrão da aplicação
EXPOSE 8080
EXPOSE 443

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "EstudandoCacheDotNet.dll"]