#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /Source
COPY ./Source/ ./
RUN dotnet restore "IIASA.FotoQuestApi.Web/IIASA.FotoQuestApi.Web.csproj"
COPY ./Source/ ./
WORKDIR "/Source/IIASA.FotoQuestApi.Web"
RUN dotnet build "IIASA.FotoQuestApi.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IIASA.FotoQuestApi.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IIASA.FotoQuestApi.Web.dll"]