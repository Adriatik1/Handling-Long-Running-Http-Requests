#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SmartSolution.API/SmartSolution.API.csproj", "SmartSolution.API/"]
COPY ["SmartCalculations.Contracts/SmartCalculations.MessageContracts.csproj", "SmartCalculations.Contracts/"]
COPY ["SmartCalculations.Consumers/SmartCalculations.MessageComponents.csproj", "SmartCalculations.Consumers/"]
RUN dotnet restore "SmartSolution.API/SmartSolution.API.csproj"
COPY . .
WORKDIR "/src/SmartSolution.API"
RUN dotnet build "SmartSolution.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartSolution.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartSolution.API.dll"]