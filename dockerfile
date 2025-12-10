FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln ./
COPY Million.API/*.csproj ./Million.API/
COPY Million.Properties.Application/*.csproj ./Million.Properties.Application/
COPY Million.Properties.Domain/*.csproj ./Million.Properties.Domain/
COPY Million.Properties.Infrastructure/*.csproj ./Million.Properties.Infrastructure/
COPY Million.Properties.Application.UnitTest/*.csproj ./Million.Properties.Application.UnitTest/
COPY Million.Properties.IntegrationTest/*.csproj ./Million.Properties.IntegrationTest/
COPY Million.Properties.Data.Test/*.csproj ./Million.Properties.Data.Test/
COPY Million.Commons/*.csproj ./Million.Commons/

RUN dotnet restore

COPY . .

WORKDIR /src/Million.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

RUN ls /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./

EXPOSE 5000

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENV MongoSettings__ConnectionString=mongodb://mongo:27017
ENV MongoSettings__DatabaseName=million

ENTRYPOINT ["dotnet", "Million.Properties.API.dll"]
