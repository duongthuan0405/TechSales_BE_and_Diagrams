FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /techsales-source

# copy csproj and restore as distinct layers
COPY ./WebAPI/*.csproj ./WebAPI/
COPY ./Modules/Test_Module/*.csproj ./Modules/Test_Module/
COPY ./Modules/Common_Module/*.csproj ./Modules/Common_Module/
COPY ./Modules/Auth_Module/*.csproj ./Modules/Auth_Module/

RUN dotnet restore ./WebAPI/WebAPI.csproj --no-cache

# copy everything else and build app
COPY ./ ./
WORKDIR /techsales-source/WebAPI
RUN dotnet publish -c release -o /techsales-publish

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /techsales-publish ./

# Environment
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8081

ENTRYPOINT ["dotnet", "WebAPI.dll"]
