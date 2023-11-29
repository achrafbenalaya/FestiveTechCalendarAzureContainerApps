#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 3500

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["hunterxhunterapi.csproj", "."]
RUN dotnet restore "./hunterxhunterapi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "hunterxhunterapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "hunterxhunterapi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:3500

ENTRYPOINT ["dotnet", "hunterxhunterapi.dll"]