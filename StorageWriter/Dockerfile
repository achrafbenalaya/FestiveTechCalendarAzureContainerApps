#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["StorageWriter.csproj", "."]
RUN dotnet restore "./StorageWriter.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "StorageWriter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StorageWriter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir /app/upload
RUN chmod -R 777 /app/upload
RUN mkdir /app/upload/images
RUN chmod -R 777 /app/upload/images
RUN mkdir /app/upload/pdf
RUN chmod -R 777 /app/upload/pdf


ENTRYPOINT ["dotnet", "StorageWriter.dll"]