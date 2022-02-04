#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NotezApp.API/NotezApp.API.csproj", "NotezApp.API/"]
COPY ["NotezApp.Business/NotezApp.Business.csproj", "NotezApp.Business/"]
COPY ["NotezApp.Domain/NotezApp.Domain.csproj", "NotezApp.Domain/"]
COPY ["NotezApp.Data/NotezApp.Data.csproj", "NotezApp.Data/"]
RUN dotnet restore "NotezApp.API/NotezApp.API.csproj"
COPY . .
WORKDIR "/src/NotezApp.API"
RUN dotnet build "NotezApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotezApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotezApp.API.dll"]