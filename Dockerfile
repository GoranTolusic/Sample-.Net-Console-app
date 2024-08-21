# Koristi oficijelnu ASP.NET Core Docker sliku za build
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Koristi oficijelnu .NET Core SDK 8.0 sliku za build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["myApp.csproj", ""]
RUN dotnet restore "myApp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "myApp.csproj" -c Release -o /app/build

# Pravimo finalni Docker image
FROM build AS publish
RUN dotnet publish "myApp.csproj" -c Release -o /app/publish

# Kreiranje finalnog imagea
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "myApp.dll"]

#docker build -t mvc .
#docker run -p 8080:80 --network=host mvc