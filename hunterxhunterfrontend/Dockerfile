#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Accept the URL as a build argument
ARG URL_ARG=
# Set the URL as an environment variable
ENV AzureLocationApi__Url=$URL_ARG

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["hunterxhunterfrontend.csproj", "."]
RUN dotnet restore "./hunterxhunterfrontend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "hunterxhunterfrontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "hunterxhunterfrontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "hunterxhunterfrontend.dll"]