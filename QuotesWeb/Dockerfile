
# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["QuotesWeb/QuotesWeb.csproj", "QuotesWeb/"]
RUN dotnet restore "QuotesWeb/QuotesWeb.csproj"
COPY . .
WORKDIR "/src/QuotesWeb"
RUN dotnet build "QuotesWeb.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "QuotesWeb.csproj" -c Release -o /app/publish

# Final image with the app ready to run
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuotesWeb.dll"]
