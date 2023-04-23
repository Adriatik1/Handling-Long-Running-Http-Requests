#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SmartSolution.Services/SmartSolution.Services.csproj", "SmartSolution.Services/"]
COPY ["SmartCalculations.Consumers/SmartCalculations.MessageComponents.csproj", "SmartCalculations.Consumers/"]
COPY ["SmartCalculations.Contracts/SmartCalculations.MessageContracts.csproj", "SmartCalculations.Contracts/"]
RUN dotnet restore "SmartSolution.Services/SmartSolution.Services.csproj"
COPY . .
WORKDIR "/src/SmartSolution.Services"
RUN dotnet build "SmartSolution.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartSolution.Services.csproj" -c Release -o /app/publish

COPY SmartSolution.Services/appsettings.json /app/appsettings.json

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartSolution.Services.dll"]