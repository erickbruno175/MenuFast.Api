# Runtime do .NET 10
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
# Pasta de trabalho dentro do container
WORKDIR /app
# Porta interna da API
EXPOSE 8080
# Faz o ASP.NET escutar na porta 8080
ENV ASPNETCORE_URLS=http://+:8080
# SDK usado para compilar
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
# Pasta do código
WORKDIR /src
# Copia o arquivo do projeto
COPY ["MenuFast.Api.csproj", "./"]
# Restaura os pacotes NuGet
RUN dotnet restore "MenuFast.Api.csproj"
# Copia todo o projeto
COPY . .
# Compila em modo Release
RUN dotnet build "MenuFast.Api.csproj" -c Release -o /app/build
# Publica a aplicação
FROM build AS publish
RUN dotnet publish "MenuFast.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
# Imagem final
FROM base AS final
WORKDIR /app
# Copia a aplicação publicada
COPY --from=publish /app/publish .
# Inicia a API
ENTRYPOINT ["dotnet", "MenuFast.Api.dll"]