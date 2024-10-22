FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["Presentation/Presentation.csproj", "Presentation/"]
RUN dotnet restore "Presentation/Presentation.csproj"

COPY . .
WORKDIR /src/Presentation
RUN dotnet publish "Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .


EXPOSE 8082

ENTRYPOINT ["dotnet","Presentation.dll"]
