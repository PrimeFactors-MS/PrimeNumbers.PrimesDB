#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["PrimeNumbers.PrimesDB.API/PrimeNumbers.PrimesDB.API.csproj", "PrimeNumbers.PrimesDB.API/"]
RUN dotnet restore "PrimeNumbers.PrimesDB.API/PrimeNumbers.PrimesDB.API.csproj"
COPY . .
WORKDIR "/src/PrimeNumbers.PrimesDB.API"
RUN dotnet build "PrimeNumbers.PrimesDB.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PrimeNumbers.PrimesDB.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrimeNumbers.PrimesDB.API.dll"]