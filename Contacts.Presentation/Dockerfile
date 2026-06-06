FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Contacts.Presentation/Contacts.Presentation.csproj", "Contacts.Presentation/"]
COPY ["Contacts.Domain/Contacts.Domain.csproj", "Contacts.Domain/"]
COPY ["Contacts.Application/Contacts.Application.csproj", "Contacts.Application/"]
COPY ["Contacts.Infrastrucure/Contacts.Infrastrucure.csproj", "Contacts.Infrastrucure/"]
RUN dotnet restore "Contacts.Presentation/Contacts.Presentation.csproj"
COPY . .
WORKDIR "/src/Contacts.Presentation"
RUN dotnet build "./Contacts.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Contacts.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contacts.Presentation.dll"]
