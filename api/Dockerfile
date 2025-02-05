# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy project files
COPY *.csproj ./
RUN dotnet restore

# Copy everything and build
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 5001
CMD ["dotnet", "backend.dll"]
