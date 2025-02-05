# Use the official .NET image from the Docker Hub
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["QuotesApi/QuotesApi.csproj", "QuotesApi/"]
RUN dotnet restore "QuotesApi/QuotesApi.csproj"
COPY . .
WORKDIR "/src/QuotesApi"
RUN dotnet build "QuotesApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuotesApi.csproj" -c Release -o /app/publish

# Copy the build artifacts to the base image and set the entry point
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuotesApi.dll"]
