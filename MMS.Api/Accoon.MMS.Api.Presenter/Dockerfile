FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Presenter/Accoon.Api/Accoon.Api.csproj", "Presenter/Accoon.Api/"]
RUN dotnet restore "Presenter/Accoon.Api/Accoon.Api.csproj"
COPY . .
WORKDIR "/src/Presenter/Accoon.Api"
RUN dotnet build "Accoon.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Accoon.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Accoon.Api.dll"]