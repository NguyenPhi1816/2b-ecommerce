# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./2b-ecommerce.csproj ./
RUN dotnet restore 2b-ecommerce.csproj

COPY . .
RUN dotnet publish 2b-ecommerce.csproj -c Release -o /app/publish /p:UseAppHost=false

# ===== Runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5049
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish ./
EXPOSE 5049

ENTRYPOINT ["dotnet", "./2b-ecommerce.dll"]
