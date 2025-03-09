# Use the official .NET 9 SDK image as a build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy the project files and restore dependencies
COPY *.sln .
COPY Auto1040.Api/*.csproj ./Auto1040.Api/
COPY Auto1040.Core/*.csproj ./Auto1040.Core/
COPY Auto1040.Service/*.csproj ./Auto1040.Service/
COPY Auto1040.Data/*.csproj ./Auto1040.Data/
RUN dotnet restore

# Copy the remaining files and build the project
COPY . .
WORKDIR /source/Auto1040.Api
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 9 ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Auto1040.Api.dll"]
