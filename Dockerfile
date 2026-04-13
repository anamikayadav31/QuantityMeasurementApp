# ── Build Stage ──────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY QuantityMeasurementApp.sln ./
COPY QuantityMeasurementApp.AspApi/QuantityMeasurementApp.AspApi.csproj                     QuantityMeasurementApp.AspApi/
COPY QuantityMeasurementApp.BussinessLayer/QuantityMeasurementApp.BussinessLayer.csproj     QuantityMeasurementApp.BussinessLayer/
COPY QuantityMeasurementApp.RepoLayer/QuantityMeasurementApp.RepoLayer.csproj               QuantityMeasurementApp.RepoLayer/
COPY QuantityMeasurementApp.ModelLayer/QuantityMeasurementApp.ModelLayer.csproj             QuantityMeasurementApp.ModelLayer/

RUN dotnet restore

COPY . .
RUN dotnet publish QuantityMeasurementApp.AspApi/QuantityMeasurementApp.AspApi.csproj \
    -c Release -o /app/publish

# ── Runtime Stage ─────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

ENTRYPOINT ["dotnet", "QuantityMeasurementApp.AspApi.dll"]