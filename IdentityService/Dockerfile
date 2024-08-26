# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем .csproj файлы и восстанавливаем зависимости
COPY ["Identity/Identity.csproj", "Identity/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infastructure/Infastructure.csproj", "Infastructure/"]
COPY ["Presentation/Presentation.csproj", "Presentation/"]
RUN dotnet restore "Identity/Identity.csproj"

# Копируем остальные файлы и выполняем сборку
COPY . .
WORKDIR /src/Identity
RUN dotnet publish "Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Конечный этап
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем собранное приложение из этапа сборки
COPY --from=build /app/publish .

# Открываем порт для доступа к приложению
EXPOSE 8080

# Устанавливаем команду для запуска приложения с ожиданием базы данных и миграцией
ENTRYPOINT ["dotnet","Identity.dll"]
