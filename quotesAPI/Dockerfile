FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["QuotesApi/QuotesApi.csproj", "QuotesApi/"]
RUN dotnet restore "QuotesApi/QuotesApi.csproj"

COPY . .
WORKDIR "/src/QuotesApi"
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
CMD ["dotnet", "QuotesApi.dll"]
